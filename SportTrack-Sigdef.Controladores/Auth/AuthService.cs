using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Auth.Dtos;
using SportTrack_Sigdef.Controladores.SaaS;
using SportTrack_Sigdef.Controladores.SaaS.Dtos;
using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
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

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string? clientApp = null)
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
                    throw new UnauthorizedException("Tu cuenta ha sido deshabilitada por superar el límite de intentos. Contactá al administrador (desarrollador) para habilitarla. Se recomienda cambiar la contraseña.");
                }

                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();

                Console.WriteLine($"CONTRASEÑA INCORRECTA para: {cleanUsername}. Quedan {intentosRestantes} intentos.");
                await _auditService.RegistrarAccionAsync("LOGIN_FAILED", $"Contraseña incorrecta para '{cleanUsername}'. Quedan {intentosRestantes} intentos.", cleanUsername, "Auth");
                throw new UnauthorizedException($"Contraseña incorrecta. Te quedan {intentosRestantes} intentos antes del bloqueo.");
            }

            // Si el login fue exitoso, reseteamos el contador
            if (user.IntentosFallidos > 0)
            {
                user.IntentosFallidos = 0;
                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();
            }

            // Verificar que la cuenta esté habilitada
            if (!user.EstaActivo)
            {
                Console.WriteLine($"CUENTA DESHABILITADA: {cleanUsername}");
                await _auditService.RegistrarAccionAsync("LOGIN_BLOCKED", $"Acceso bloqueado: cuenta '{cleanUsername}' está deshabilitada.", cleanUsername, "Auth");
                throw new UnauthorizedException("Tu cuenta está temporalmente deshabilitada. Contactá al administrador.");
            }

            // SaaS Enforcement: Verificar si la entidad está activa y pagos
            // Club afiliado hereda estado SaaS de la federación padre.
            if (user.RolFederacion != "SuperAdmin" && (user.Club != null || user.Federacion != null))
            {
                var fedScope = user.Federacion ?? user.Club?.Federacion;
                bool activo = fedScope?.Activo ?? user.Club?.Activo ?? true;
                bool bloqueado = fedScope?.BloqueadaPorFaltaDePago ?? user.Club?.BloqueadoPorFaltaDePago ?? false;
                DateTime? vencimiento = fedScope?.FechaVencimientoPlan ?? user.Club?.FechaVencimientoPlan;
                string nombreInst = fedScope?.Nombre ?? user.Club?.Nombre ?? "";

                if (!activo)
                {
                    Console.WriteLine($"ENTIDAD SUSPENDIDA: {nombreInst} para usuario {cleanUsername}");
                    await _auditService.RegistrarAccionAsync("LOGIN_BLOCKED", $"Acceso bloqueado: '{nombreInst}' está suspendida.", cleanUsername, "Auth");
                    throw new UnauthorizedException("El acceso de tu institución ha sido suspendido temporalmente por el administrador del sistema.");
                }

                if (bloqueado)
                {
                    Console.WriteLine($"ENTIDAD BLOQUEADA POR PAGO: {nombreInst} para usuario {cleanUsername}");
                    await _auditService.RegistrarAccionAsync("LOGIN_BLOCKED", $"Acceso bloqueado: '{nombreInst}' está bloqueada por falta de pago.", cleanUsername, "Auth");
                    throw new UnauthorizedException("El acceso de tu institución se encuentra bloqueado por falta de pago. Por favor, regularice su situación.");
                }

                if (vencimiento.HasValue && vencimiento.Value.Date < DateTime.UtcNow.Date)
                {
                    Console.WriteLine($"ENTIDAD VENCIDA: {nombreInst} para usuario {cleanUsername}");
                    await _auditService.RegistrarAccionAsync("LOGIN_BLOCKED", $"Acceso bloqueado: la suscripción de '{nombreInst}' se encuentra vencida.", cleanUsername, "Auth");
                    throw new UnauthorizedException("La suscripción de tu institución ha vencido. Por favor, regularice el pago para reactivar el acceso.");
                }
            }

            Console.WriteLine($"LOGIN EXITOSO: {cleanUsername}");

            var response = _mapper.Map<AuthResponseDto>(user);
            
            var planSaaSAsignado = await ResolvePlanForUserAsync(user);

            if (planSaaSAsignado != null)
            {
                response.Plan = PlanSaaSAccessHelper.FromEntity(planSaaSAsignado);
            }

            // Bloqueo SIGDEF: plan solo SportTrack (o sin plan) no puede usar esta app
            if (string.Equals(clientApp, "sigdef", StringComparison.OrdinalIgnoreCase)
                && user.RolFederacion != "SuperAdmin"
                && (user.Club != null || user.Federacion != null)
                && (response.Plan == null || !response.Plan.AccesoSigdef))
            {
                var planNombre = response.Plan?.Nombre ?? "sin plan asignado";
                throw new UnauthorizedException(
                    $"Tu plan actual ({planNombre}) no incluye acceso al sistema SIGDEF. Actualizá a un plan SIGDEF o Pack Dúo.");
            }

            // Bloqueo SportTrack: plan solo SIGDEF no puede usar esta app
            if (string.Equals(clientApp, "sporttrack", StringComparison.OrdinalIgnoreCase)
                && user.RolFederacion != "SuperAdmin"
                && (user.Club != null || user.Federacion != null)
                && (response.Plan == null || !response.Plan.AccesoSportTrack))
            {
                var planNombre = response.Plan?.Nombre ?? "sin plan asignado";
                throw new UnauthorizedException(
                    $"Tu plan actual ({planNombre}) no incluye acceso al sistema SportTrack. Actualizá a un plan SportTrack o Pack Dúo.");
            }

            if (user.Federacion != null)
            {
                response.FechaVencimientoPlan = user.Federacion.FechaVencimientoPlan;
            }
            else if (user.Club?.Federacion != null)
            {
                response.FechaVencimientoPlan = user.Club.Federacion.FechaVencimientoPlan;
                response.FrecuenciaPago = user.Club.FrecuenciaPago;
            }
            else if (user.Club != null)
            {
                response.FrecuenciaPago = user.Club.FrecuenciaPago;
                response.FechaVencimientoPlan = user.Club.FechaVencimientoPlan;
            }

            response.Token = _tokenService.CreateToken(user);
            
            await _auditService.RegistrarAccionAsync("LOGIN_SUCCESS", $"Usuario '{user.Username}' inició sesión correctamente.", user.Username, "Auth");

            return response;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            if (await UserExistsAsync(registerDto.Username))
                throw new BadRequestException("El nombre de usuario ya existe");

            var rol = (registerDto.RolFederacion ?? "Club").Trim();

            if (string.Equals(rol, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
                throw new BadRequestException("No se puede crear un SuperAdmin desde la API.");

            if (!AuthRolePolicies.RegisterableRoles.Any(r =>
                    string.Equals(r, rol, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BadRequestException(
                    $"Rol no permitido: '{rol}'. Roles válidos: {string.Join(", ", AuthRolePolicies.RegisterableRoles)}.");
            }

            // Normalizar casing al catálogo
            rol = AuthRolePolicies.RegisterableRoles.First(r =>
                string.Equals(r, rol, StringComparison.OrdinalIgnoreCase));

            var isClubRole = string.Equals(rol, "Club", StringComparison.OrdinalIgnoreCase);

            if (isClubRole && (!registerDto.ClubId.HasValue || registerDto.ClubId.Value <= 0))
                throw new BadRequestException("Un usuario con rol Club debe estar vinculado a un club.");

            int? clubId = registerDto.ClubId is > 0 ? registerDto.ClubId : null;
            int? federacionId = registerDto.FederacionId is > 0 ? registerDto.FederacionId : null;

            // Siempre alinear IdFederacion con el club (fuente de verdad)
            if (clubId.HasValue)
            {
                var club = await _context.Clubes.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.IdClub == clubId.Value)
                    ?? throw new BadRequestException($"El club con ID {clubId} no existe.");

                if (!club.IdFederacion.HasValue)
                    throw new BadRequestException("El club debe pertenecer a una federación.");

                federacionId = club.IdFederacion;
            }

            if (isClubRole && !federacionId.HasValue)
                throw new BadRequestException("No se pudo resolver la federación del club para el login.");

            if (PlanSaaSAccessHelper.IsJudgeRole(rol) && !federacionId.HasValue)
                throw new BadRequestException("Un usuario juez debe estar vinculado a una federación.");

            // Enforcement: Club / jueces según plan de la federación destino
            if (federacionId.HasValue &&
                (isClubRole || PlanSaaSAccessHelper.IsJudgeRole(rol)))
            {
                var federacion = await _context.Federaciones.AsNoTracking()
                    .Include(f => f.PlanSaaS)
                    .FirstOrDefaultAsync(f => f.IdFederacion == federacionId.Value)
                    ?? throw new BadRequestException($"La federación con ID {federacionId} no existe.");

                if (federacion.PlanSaaS == null)
                    throw new BadRequestException("La federación no tiene un plan SaaS asignado.");

                var planDto = PlanSaaSAccessHelper.FromEntity(federacion.PlanSaaS);
                if (!PlanSaaSAccessHelper.CanCreateRole(planDto, rol))
                {
                    if (isClubRole)
                        throw new BadRequestException(
                            $"El plan '{planDto.Nombre}' no incluye dashboard/login Club. Actualizá a Profesional o superior.");
                    throw new BadRequestException(
                        $"El plan '{planDto.Nombre}' no incluye consolas de juez (Largador/Cronometrista/Juez de Control). Requiere Ecosistema SportTrack o Pack Dúo.");
                }
            }

            var user = _mapper.Map<Usuario>(registerDto);
            user.Username = registerDto.Username.ToLower().Trim();
            user.RolFederacion = rol;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            user.IdClub = clubId;
            user.IdFederacion = federacionId;

            _context.Usuarios.Add(user);
            var res = await _context.SaveChangesAsync() > 0;

            if (res)
            {
                await _auditService.RegistrarAccionAsync("REGISTER_USER",
                    $"Nuevo usuario registrado: '{user.Username}' (Rol: {user.RolFederacion}, ClubId: {user.IdClub}, FedId: {user.IdFederacion})", null, "Auth");
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
                    // Un Admin de Federación solo ve:
                    // 1. Usuarios de su propia Federación
                    // 2. Usuarios de sus Clubes Afiliados
                    // 3. SuperAdmin (para mensajería)
                    var fedId = requester.IdFederacion.Value;
                    query = query.Where(u =>
                        u.IdFederacion == fedId
                        || (u.Club != null && u.Club.IdFederacion == fedId)
                        || u.RolFederacion == "SuperAdmin");
                }
                else if (requester != null && requester.RolFederacion != "SuperAdmin" && requester.RolFederacion != "soporte_tecnico")
                {
                    // Club: ve usuarios de su club + Admins de su federación (para mensajería)
                    if (string.Equals(requester.RolFederacion, "Club", StringComparison.OrdinalIgnoreCase))
                    {
                        var clubFedId = requester.IdFederacion
                            ?? (requester.IdClub.HasValue
                                ? (await _context.Clubes.AsNoTracking()
                                    .Where(c => c.IdClub == requester.IdClub.Value)
                                    .Select(c => c.IdFederacion)
                                    .FirstOrDefaultAsync())
                                : null);

                        query = query.Where(u =>
                            (requester.IdClub.HasValue && u.IdClub == requester.IdClub)
                            || (clubFedId.HasValue
                                && u.RolFederacion == "Admin"
                                && u.IdFederacion == clubFedId.Value));
                    }
                    else if (requester.IdClub.HasValue)
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

        public async Task<UsuarioDto> GetMeAsync(string username, string? clientApp = null)
        {
            var user = await _context.Usuarios
                .Include(u => u.Federacion)
                    .ThenInclude(f => f!.PlanSaaS)
                .Include(u => u.Club)
                    .ThenInclude(c => c!.PlanSaaS)
                .Include(u => u.Club)
                    .ThenInclude(c => c!.Federacion)
                        .ThenInclude(f => f!.PlanSaaS)
                .FirstOrDefaultAsync(u => u.Username == username.ToLower());

            if (user == null) throw new NotFoundException("Usuario no encontrado");

            // SaaS Enforcement en tiempo real
            if (user.RolFederacion != "SuperAdmin" && (user.Club != null || user.Federacion != null))
            {
                var fedScope = user.Federacion ?? user.Club?.Federacion;
                bool activo = fedScope?.Activo ?? user.Club?.Activo ?? true;
                bool bloqueado = fedScope?.BloqueadaPorFaltaDePago ?? user.Club?.BloqueadoPorFaltaDePago ?? false;
                DateTime? vencimiento = fedScope?.FechaVencimientoPlan ?? user.Club?.FechaVencimientoPlan;

                if (!activo)
                {
                    throw new UnauthorizedException("El acceso de tu institución ha sido suspendido.");
                }

                if (bloqueado || (vencimiento.HasValue && vencimiento.Value.Date < DateTime.UtcNow.Date))
                {
                    throw new UnauthorizedException("La suscripción de tu institución ha vencido o está bloqueada por falta de pago.");
                }
            }

            var response = _mapper.Map<UsuarioDto>(user);

            PlanSaaS? planSaaSAsignado = await ResolvePlanForUserAsync(user);

            if (user.Federacion != null)
            {
                response.FechaVencimientoPlan = user.Federacion.FechaVencimientoPlan;
            }
            else if (user.Club?.Federacion != null)
            {
                response.FechaVencimientoPlan = user.Club.Federacion.FechaVencimientoPlan;
                response.FrecuenciaPago = user.Club.FrecuenciaPago;
            }
            else if (user.Club != null)
            {
                response.FrecuenciaPago = user.Club.FrecuenciaPago;
                response.FechaVencimientoPlan = user.Club.FechaVencimientoPlan;
            }

            if (planSaaSAsignado != null)
            {
                response.Plan = PlanSaaSAccessHelper.FromEntity(planSaaSAsignado);
            }

            if (string.Equals(clientApp, "sigdef", StringComparison.OrdinalIgnoreCase)
                && user.RolFederacion != "SuperAdmin"
                && (user.Club != null || user.Federacion != null)
                && (response.Plan == null || !response.Plan.AccesoSigdef))
            {
                var planNombre = response.Plan?.Nombre ?? "sin plan asignado";
                throw new UnauthorizedException(
                    $"Tu plan actual ({planNombre}) no incluye acceso al sistema SIGDEF. Actualizá a un plan SIGDEF o Pack Dúo.");
            }

            if (string.Equals(clientApp, "sporttrack", StringComparison.OrdinalIgnoreCase)
                && user.RolFederacion != "SuperAdmin"
                && (user.Club != null || user.Federacion != null)
                && (response.Plan == null || !response.Plan.AccesoSportTrack))
            {
                var planNombre = response.Plan?.Nombre ?? "sin plan asignado";
                throw new UnauthorizedException(
                    $"Tu plan actual ({planNombre}) no incluye acceso al sistema SportTrack. Actualizá a un plan SportTrack o Pack Dúo.");
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

        /// <summary>
        /// Plan SaaS siempre vive en la federación.
        /// Admin → plan de su federación. Club → plan de la federación a la que pertenece.
        /// </summary>
        private async Task<PlanSaaS?> ResolvePlanForUserAsync(Usuario user)
        {
            int? federacionId = user.IdFederacion
                ?? user.Federacion?.IdFederacion
                ?? user.Club?.IdFederacion;

            if (!federacionId.HasValue && user.IdClub.HasValue)
            {
                var club = user.Club ?? await _context.Clubes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.IdClub == user.IdClub.Value);
                federacionId = club?.IdFederacion;
            }

            if (!federacionId.HasValue)
                return null;

            var fed = (user.Federacion?.IdFederacion == federacionId.Value ? user.Federacion : null)
                ?? (user.Club?.Federacion?.IdFederacion == federacionId.Value ? user.Club.Federacion : null)
                ?? await _context.Federaciones
                    .Include(f => f.PlanSaaS)
                    .FirstOrDefaultAsync(f => f.IdFederacion == federacionId.Value);

            if (fed?.PlanSaaS != null)
                return fed.PlanSaaS;

            if (fed?.PlanSaaSId is int planId)
                return await _context.PlanesSaaS.FindAsync(planId);

            return null;
        }
    }
}
