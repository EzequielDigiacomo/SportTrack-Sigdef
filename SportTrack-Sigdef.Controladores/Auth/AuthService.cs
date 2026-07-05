using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Auth.Dtos;
using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Entidades.Entidades;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SportTrackDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly Audit.IAuditService _auditService;

        public AuthService(SportTrackDbContext context, ITokenService tokenService, IMapper mapper, Audit.IAuditService auditService)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
            _auditService = auditService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var cleanUsername = loginDto.Username.Trim().ToLower();
            var cleanPassword = loginDto.Password.Trim();

            Console.WriteLine($"--- INTENTO DE LOGIN: {cleanUsername} ---");

            var user = await _context.Usuarios
                .Include(u => u.Federacion)
                    .ThenInclude(f => f.PlanSaaS)
                .Include(u => u.Club)
                    .ThenInclude(c => c.Federacion)
                .Include(u => u.Club)
                    .ThenInclude(c => c.PlanSaaS)
                .FirstOrDefaultAsync(u => u.Username == cleanUsername);

            if (user == null) 
            {
                Console.WriteLine($"USUARIO NO ENCONTRADO: {cleanUsername}");
                await _auditService.RegistrarAccionAsync("LOGIN_FAILED", $"Intento fallido: Usuario '{cleanUsername}' no encontrado.", cleanUsername, "Auth");
                throw new UnauthorizedException("Usuario no encontrado en la base de datos");
            }

            Console.WriteLine($"USUARIO ENCONTRADO. Verificando hash para: {cleanUsername}");

            var passwordValid = false;
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                try
                {
                    passwordValid = BCrypt.Net.BCrypt.Verify(cleanPassword, user.PasswordHash);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hash inválido para {cleanUsername}: {ex.Message}");
                }
            }

            if (!passwordValid)
            {
                user.IntentosFallidos++;
                var intentosRestantes = 5 - user.IntentosFallidos;
                
                if (intentosRestantes <= 0)
                {
                    user.EstaActivo = false;
                    user.IntentosFallidos = 0;
                    _context.Usuarios.Update(user);
                    await _context.SaveChangesAsync();
                    
                    await _auditService.RegistrarAccionAsync("ACCOUNT_LOCKED", $"Cuenta '{cleanUsername}' bloqueada por 5 intentos fallidos.", cleanUsername, "Auth");
                    throw new UnauthorizedException("Tu cuenta ha sido deshabilitada por superar el lÃ­mite de intentos. ContactÃ¡ al administrador (desarrollador) para habilitarla. Se recomienda cambiar la contraseÃ±a.");
                }

                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();

                Console.WriteLine($"CONTRASEÃ‘A INCORRECTA para: {cleanUsername}. Quedan {intentosRestantes} intentos.");
                await _auditService.RegistrarAccionAsync("LOGIN_FAILED", $"ContraseÃ±a incorrecta para '{cleanUsername}'. Quedan {intentosRestantes} intentos.", cleanUsername, "Auth");
                throw new UnauthorizedException($"ContraseÃ±a incorrecta. Te quedan {intentosRestantes} intentos antes del bloqueo.");
            }

            // Si el login fue exitoso, reseteamos el contador
            if (user.IntentosFallidos > 0)
            {
                user.IntentosFallidos = 0;
                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();
            }

            // Verificar que la cuenta estÃ© habilitada
            if (!user.EstaActivo)
            {
                Console.WriteLine($"CUENTA DESHABILITADA: {cleanUsername}");
                await _auditService.RegistrarAccionAsync("LOGIN_BLOCKED", $"Acceso bloqueado: cuenta '{cleanUsername}' estÃ¡ deshabilitada.", cleanUsername, "Auth");
                throw new UnauthorizedException("Tu cuenta estÃ¡ temporalmente deshabilitada. ContactÃ¡ al administrador.");
            }

            // SaaS Enforcement: Verificar si la entidad estÃ¡ activa y pagos
            if (user.RolFederacion != "SuperAdmin" && (user.Club != null || user.Federacion != null))
            {
                bool activo = user.Federacion?.Activo ?? user.Club?.Activo ?? true;
                bool bloqueado = user.Federacion?.BloqueadaPorFaltaDePago ?? user.Club?.BloqueadoPorFaltaDePago ?? false;
                DateTime? vencimiento = user.Federacion?.FechaVencimientoPlan ?? user.Club?.FechaVencimientoPlan;
                string nombreInst = user.Federacion?.Nombre ?? user.Club?.Nombre ?? "";

                if (!activo)
                {
                    Console.WriteLine($"ENTIDAD SUSPENDIDA: {nombreInst} para usuario {cleanUsername}");
                    await _auditService.RegistrarAccionAsync("LOGIN_BLOCKED", $"Acceso bloqueado: '{nombreInst}' estÃ¡ suspendida.", cleanUsername, "Auth");
                    throw new UnauthorizedException("El acceso de tu instituciÃ³n ha sido suspendido temporalmente por el administrador del sistema.");
                }

                if (bloqueado)
                {
                    Console.WriteLine($"ENTIDAD BLOQUEADA POR PAGO: {nombreInst} para usuario {cleanUsername}");
                    await _auditService.RegistrarAccionAsync("LOGIN_BLOCKED", $"Acceso bloqueado: '{nombreInst}' estÃ¡ bloqueada por falta de pago.", cleanUsername, "Auth");
                    throw new UnauthorizedException("El acceso de tu instituciÃ³n se encuentra bloqueado por falta de pago. Por favor, regularice su situaciÃ³n.");
                }

                if (vencimiento.HasValue && vencimiento.Value.Date < DateTime.UtcNow.Date)
                {
                    Console.WriteLine($"ENTIDAD VENCIDA: {nombreInst} para usuario {cleanUsername}");
                    await _auditService.RegistrarAccionAsync("LOGIN_BLOCKED", $"Acceso bloqueado: la suscripciÃ³n de '{nombreInst}' se encuentra vencida.", cleanUsername, "Auth");
                    throw new UnauthorizedException("La suscripciÃ³n de tu instituciÃ³n ha vencido. Por favor, regularice el pago para reactivar el acceso.");
                }
            }

            Console.WriteLine($"LOGIN EXITOSO: {cleanUsername}");

            var response = _mapper.Map<AuthResponseDto>(user);
            
            PlanSaaS? planSaaSAsignado = user.Federacion?.PlanSaaS ?? user.Club?.PlanSaaS;
            
            // Si el club no tiene plan, hereda de la federaciÃ³n
            if (planSaaSAsignado == null && user.Club?.IdFederacion != null)
            {
                var parentFed = await _context.Federaciones
                    .Include(f => f.PlanSaaS)
                    .FirstOrDefaultAsync(f => f.IdFederacion == user.Club.IdFederacion);
                planSaaSAsignado = parentFed?.PlanSaaS;
            }

            if (planSaaSAsignado != null)
            {
                response.Plan = _mapper.Map<SportTrack_Sigdef.Controladores.SaaS.Dtos.PlanSaaSDto>(planSaaSAsignado);
            }
            
            // Usamos las variables unificadas si el plan existe
            if (user.Federacion != null)
            {
                response.FechaVencimientoPlan = user.Federacion.FechaVencimientoPlan;
            }
            else if (user.Club != null)
            {
                response.FrecuenciaPago = user.Club.FrecuenciaPago;
                response.FechaVencimientoPlan = user.Club.FechaVencimientoPlan;
            }
            response.Token = _tokenService.CreateToken(user);
            
            await _auditService.RegistrarAccionAsync("LOGIN_SUCCESS", $"Usuario '{user.Username}' iniciÃ³ sesiÃ³n correctamente.", user.Username, "Auth");

            return response;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            if (await UserExistsAsync(registerDto.Username))
                throw new BadRequestException("El nombre de usuario ya existe");

            var user = _mapper.Map<Usuario>(registerDto);
            user.Username = registerDto.Username.ToLower();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            
            _context.Usuarios.Add(user);
            var res = await _context.SaveChangesAsync() > 0;

            if (res)
            {
                await _auditService.RegistrarAccionAsync("REGISTER_USER", 
                    $"Nuevo usuario registrado: '{user.Username}' (RolFederacion: {user.RolFederacion})", null, "Auth");
            }

            return res;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Usuarios.AnyAsync(u => u.Username == username.ToLower());
        }

        public async Task<System.Collections.Generic.IEnumerable<UsuarioDto>> GetUsuariosAsync(string? requesterUsername = null)
        {
            var query = _context.Usuarios
                .Include(u => u.Federacion)
                .Include(u => u.Club)
                    .ThenInclude(c => c.Federacion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(requesterUsername))
            {
                var requester = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == requesterUsername.ToLower());
                if (requester != null && requester.RolFederacion == "Admin" && requester.IdFederacion.HasValue)
                {
                    // Un Admin de FederaciÃ³n solo ve:
                    // 1. Usuarios de su propia FederaciÃ³n
                    // 2. Usuarios de sus Clubes Afiliados
                    var fedId = requester.IdFederacion.Value;
                    query = query.Where(u => u.IdFederacion == fedId || (u.Club != null && u.Club.IdFederacion == fedId));
                }
                else if (requester != null && requester.RolFederacion != "SuperAdmin" && requester.RolFederacion != "soporte_tecnico")
                {
                    // Otros roles menores solo ven sus propios datos o los de su club/federacion
                    if (requester.IdClub.HasValue)
                        query = query.Where(u => u.IdClub == requester.IdClub);
                    else if (requester.IdFederacion.HasValue)
                        query = query.Where(u => u.IdFederacion == requester.IdFederacion);
                    else
                        query = query.Where(u => u.IdUsuario == requester.IdUsuario);
                }
            }

            var usuarios = await query.ToListAsync();
            return _mapper.Map<System.Collections.Generic.IEnumerable<UsuarioDto>>(usuarios);
        }

        public async Task<bool> UpdatePasswordAsync(int id, string newPassword)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"Usuario con ID {id} no encontrado");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.Usuarios.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<UsuarioDto> GetMeAsync(string username)
        {
            var user = await _context.Usuarios
                .Include(u => u.Federacion)
                .Include(u => u.Club)
                    .ThenInclude(c => c.Federacion)
                .FirstOrDefaultAsync(u => u.Username == username.ToLower());

            if (user == null) throw new NotFoundException("Usuario no encontrado");

            // SaaS Enforcement en tiempo real
            if (user.RolFederacion != "SuperAdmin" && (user.Club != null || user.Federacion != null))
            {
                bool activo = user.Federacion?.Activo ?? user.Club?.Activo ?? true;
                bool bloqueado = user.Federacion?.BloqueadaPorFaltaDePago ?? user.Club?.BloqueadoPorFaltaDePago ?? false;
                DateTime? vencimiento = user.Federacion?.FechaVencimientoPlan ?? user.Club?.FechaVencimientoPlan;

                if (!activo)
                {
                    throw new UnauthorizedException("El acceso de tu instituciÃ³n ha sido suspendido.");
                }

                if (bloqueado || (vencimiento.HasValue && vencimiento.Value.Date < DateTime.UtcNow.Date))
                {
                    throw new UnauthorizedException("La suscripciÃ³n de tu instituciÃ³n ha vencido o estÃ¡ bloqueada por falta de pago.");
                }
            }

            var response = _mapper.Map<UsuarioDto>(user);

            if (user.Federacion != null)
            {
                response.FechaVencimientoPlan = user.Federacion.FechaVencimientoPlan;
            }
            else if (user.Club != null)
            {
                response.FrecuenciaPago = user.Club.FrecuenciaPago;
                response.FechaVencimientoPlan = user.Club.FechaVencimientoPlan;
            }

            return response;
        }

        public async Task<bool> ToggleActivoAsync(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {id} no encontrado");

            user.EstaActivo = !user.EstaActivo;
            _context.Usuarios.Update(user);
            var result = await _context.SaveChangesAsync() > 0;

            var accion = user.EstaActivo ? "USUARIO_HABILITADO" : "USUARIO_DESHABILITADO";
            await _auditService.RegistrarAccionAsync(accion,
                $"Cuenta '{user.Username}' (RolFederacion: {user.RolFederacion}) {(user.EstaActivo ? "habilitada" : "deshabilitada")} por administrador.",
                null, "Auth");

            return result;
        }

        public async Task<bool> UpdatePerfilAsync(int id, UpdatePerfilDto dto)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"Usuario con ID {id} no encontrado");
            }

            user.Nombre = dto.Nombre;
            user.Apellido = dto.Apellido;
            user.Dni = dto.Dni;
            user.Telefono = dto.Telefono;
            if (!string.IsNullOrEmpty(dto.Email))
            {
                user.Email = dto.Email;
            }

            _context.Usuarios.Update(user);
            var result = await _context.SaveChangesAsync() > 0;

            await _auditService.RegistrarAccionAsync("UPDATE_PROFILE", 
                $"Perfil actualizado para el usuario '{user.Username}'", null, "Auth");

            return result;
        }
    }
}
