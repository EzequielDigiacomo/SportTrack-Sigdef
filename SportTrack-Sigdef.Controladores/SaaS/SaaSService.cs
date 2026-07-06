using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.SaaS.Dtos;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using SportTrack_Sigdef.Controladores.Audit;

namespace SportTrack_Sigdef.Controladores.SaaS
{
    public class SaaSService : ISaaSService
    {
        private readonly SportTrackDbContext _context;
        private readonly IAuditService _auditService;

        public SaaSService(SportTrackDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<IEnumerable<PlanSaaSDto>> GetPlanesAsync()
        {
            var planes = await _context.PlanesSaaS.ToListAsync();
            return planes.Select(PlanSaaSAccessHelper.FromEntity);
        }

        public async Task<PlanSaaSDto> GetPlanByIdAsync(int id)
        {
            var p = await _context.PlanesSaaS.FindAsync(id);
            if (p == null) return null;

            return PlanSaaSAccessHelper.FromEntity(p);
        }

        public async Task AsignarPlanAClubAsync(int federacionId, int planId)
        {
            var fed = await _context.Federaciones.FindAsync(federacionId);
            if (fed != null)
            {
                var oldPlanId = fed.PlanSaaSId;
                fed.PlanSaaSId = planId;
                await _context.SaveChangesAsync();

                if (oldPlanId != planId)
                {
                    var plan = await _context.PlanesSaaS.FindAsync(planId);
                    string planNombre = plan?.Nombre ?? $"Plan ID {planId}";
                    await _auditService.RegistrarAccionAsync(
                        "ASSIGN_PLAN",
                        $"Asignado Plan '{planNombre}' a la federación '{fed.Nombre}'.",
                        modulo: "SaaS"
                    );
                }
            }
        }

        public async Task<IEnumerable<ClubSaaSStatusDto>> GetClubesStatusAsync()
        {
            // Plan basico por defecto si no tiene plan (ID 1)
            var planBasico = await _context.PlanesSaaS.FirstOrDefaultAsync(p => p.Id == 1);

            var federaciones = await _context.Federaciones
                .Include(f => f.PlanSaaS)
                .Include(f => f.Usuarios)
                .Include(f => f.Clubes)
                    .ThenInclude(c => c.Participantes)
                .Include(f => f.Clubes)
                    .ThenInclude(c => c.Usuarios)
                .ToListAsync();

            // Buscamos todos los torneos activos para agruparlos por federación madre
            var eventosActivos = await _context.Eventos
                .Where(e => (e.Estado == Entidades.Enums.EstadoEventoEnum.Programada || e.Estado == Entidades.Enums.EstadoEventoEnum.EnCurso) && e.IdFederacion.HasValue)
                .Select(e => new { e.IdFederacion, Id = e.IdEvento, e.Nombre, e.Fecha, Estado = e.Estado.ToString() })
                .ToListAsync();

            return federaciones.Select(c => 
            {
                var planActivo = c.PlanSaaS ?? planBasico;
                var maxAtletas = planActivo?.MaxAtletas ?? 500;
                var maxTorneos = planActivo?.MaxTorneosActivos ?? 1;

                var atletasRegistrados = c.Clubes.Sum(a => a.Participantes.Count);
                var usuariosCount = c.Usuarios.Count + c.Clubes.Sum(a => a.Usuarios.Count);
                
                var torneosDetalle = eventosActivos
                    .Where(e => e.IdFederacion == c.IdFederacion)
                    .Select(e => new TorneoSaaSDetailDto { Id = e.Id, Nombre = e.Nombre, Fecha = e.Fecha, Estado = e.Estado })
                    .ToList();
                
                var torneosActivosCount = torneosDetalle.Count;

                var alDia = true;
                if (maxAtletas != -1 && atletasRegistrados > maxAtletas) alDia = false;
                if (maxTorneos != -1 && torneosActivosCount > maxTorneos) alDia = false;
                if (c.FechaVencimientoPlan.HasValue && c.FechaVencimientoPlan.Value.Date < DateTime.UtcNow.Date) alDia = false;
                if (c.BloqueadaPorFaltaDePago) alDia = false;

                return new ClubSaaSStatusDto
                {
                    ClubId = c.IdFederacion,
                    ClubNombre = c.Nombre,
                    Sigla = c.Sigla,
                    Email = c.Email,
                    Telefono = c.Telefono,
                    Direccion = c.Direccion,
                    Ubicacion = "",
                    PlanSaaSId = planActivo?.Id,
                    PlanNombre = planActivo?.Nombre ?? "Desconocido",
                    MaxAtletas = maxAtletas,
                    AtletasRegistrados = atletasRegistrados,
                    ClubesAfiliadosCount = c.Clubes.Count,
                    UsuariosCount = usuariosCount,
                    MaxTorneos = maxTorneos,
                    TorneosActivosCount = torneosActivosCount,
                    TorneosActivos = torneosDetalle,
                    PlanAlDia = alDia,
                    Activo = c.Activo,
                    FrecuenciaPago = "",
                    FechaAltaPlan = c.FechaAltaPlan,
                    FechaVencimientoPlan = c.FechaVencimientoPlan,
                    BloqueadoPorFaltaDePago = c.BloqueadaPorFaltaDePago
                };
            });
        }

        public async Task ToggleClubActivoAsync(int federacionId)
        {
            var fed = await _context.Federaciones.FindAsync(federacionId);
            if (fed != null)
            {
                fed.Activo = !fed.Activo;
                await _context.SaveChangesAsync();

                string status = fed.Activo ? "habilitado" : "suspendido";
                string accion = fed.Activo ? "ACTIVATE_FEDERATION" : "SUSPEND_FEDERATION";
                await _auditService.RegistrarAccionAsync(
                    accion,
                    $"Acceso a la federación '{fed.Nombre}' {status} manualmente.",
                    modulo: "SaaS"
                );
            }
        }

        public async Task<int> CreateFederacionWithAdminAsync(SaaSCreateFederacionDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try 
            {
                var fed = new Entidades.Entidades.Federacion
                {
                    Nombre = dto.Nombre,
                    Sigla = dto.Sigla,
                    Email = dto.Email,
                    Telefono = dto.Telefono,
                    Direccion = dto.Direccion,
                    Activo = true,
                    PlanSaaSId = 1,
                    FechaAltaPlan = DateTime.UtcNow.Date,
                    FechaVencimientoPlan = DateTime.UtcNow.Date.AddMonths(1)
                };
                
                _context.Federaciones.Add(fed);
                await _context.SaveChangesAsync();

                var user = new Entidades.Entidades.Usuario
                {
                    Username = dto.AdminUsername.Trim().ToLower(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.AdminPassword),
                    Email = dto.AdminEmail, 
                    RolFederacion = "Admin",
                    IdFederacion = fed.IdFederacion,
                    EstaActivo = true
                };

                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return fed.IdFederacion;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var innerMsg = ex.InnerException?.Message ?? "";
                
                string userFriendlyMessage = "Error interno al guardar los datos.";
                
                if (innerMsg.Contains("23505") || innerMsg.Contains("duplicate key"))
                {
                    if (innerMsg.Contains("IX_Usuarios_Username"))
                        userFriendlyMessage = "El nombre de usuario administrador ya está en uso. Por favor, elige otro.";
                    else if (innerMsg.Contains("IX_Usuarios_Email"))
                        userFriendlyMessage = "El email del administrador ya está registrado en otra cuenta. Debe ser único.";
                    else if (innerMsg.Contains("IX_Federaciones_Nombre") || innerMsg.Contains("IX_Clubes_Nombre"))
                        userFriendlyMessage = "Ya existe una federación o club con ese nombre.";
                    else
                        userFriendlyMessage = "Un dato ingresado ya existe en el sistema y no puede duplicarse.";
                        
                    throw new Exception(userFriendlyMessage);
                }

                throw new Exception($"Error al crear la federación: {ex.Message}");
            }
        }

        public async Task<GlobalMetricsDto> GetGlobalMetricsAsync()
        {
            var federaciones = await _context.Federaciones
                .Include(f => f.PlanSaaS)
                .ToListAsync();
            var totalAtletas = await _context.Participantes.CountAsync();
            var totalClubes = await _context.Clubes.CountAsync();
            var torneosActivos = await _context.Eventos.CountAsync(e => e.Estado != EstadoEventoEnum.Finalizado);

            var hoy = DateTime.UtcNow.Date;
            var federacionesActivas = federaciones
                .Where(f => f.Activo && !f.BloqueadaPorFaltaDePago)
                .Where(f => !f.FechaVencimientoPlan.HasValue || f.FechaVencimientoPlan.Value.Date >= hoy)
                .ToList();

            var ingresosMensuales = federacionesActivas
                .Where(f => f.PlanSaaS != null)
                .Sum(f => f.PlanSaaS!.Precio);

            var mesesEtiquetas = new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
            var inicioVentana = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(-5);
            var altasPorMes = await _context.AtletasFederados
                .Where(a => a.FechaCreacion >= inicioVentana)
                .GroupBy(a => new { a.FechaCreacion.Year, a.FechaCreacion.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Cantidad = g.Count() })
                .ToListAsync();

            var crecimiento = Enumerable.Range(0, 6)
                .Select(offset =>
                {
                    var mes = inicioVentana.AddMonths(offset);
                    var count = altasPorMes
                        .FirstOrDefault(x => x.Year == mes.Year && x.Month == mes.Month)?.Cantidad ?? 0;
                    return new MonthlyGrowthDto
                    {
                        Mes = mesesEtiquetas[mes.Month - 1],
                        Cantidad = count
                    };
                })
                .ToList();

            var mesActual = new DateTime(hoy.Year, hoy.Month, 1);
            var mesAnterior = mesActual.AddMonths(-1);
            var altasMesActual = altasPorMes.FirstOrDefault(x => x.Year == mesActual.Year && x.Month == mesActual.Month)?.Cantidad ?? 0;
            var altasMesAnterior = altasPorMes.FirstOrDefault(x => x.Year == mesAnterior.Year && x.Month == mesAnterior.Month)?.Cantidad ?? 0;
            var porcentajeCrecimiento = altasMesAnterior > 0
                ? Math.Round((decimal)(altasMesActual - altasMesAnterior) / altasMesAnterior * 100, 1)
                : (altasMesActual > 0 ? 100m : 0m);

            var distribucionPlanes = federaciones
                .GroupBy(f => f.PlanSaaS?.Nombre ?? "Sin plan")
                .Select(g => new PlanDistributionDto
                {
                    Nombre = g.Key,
                    Cantidad = g.Count(),
                    Precio = g.First().PlanSaaS?.Precio ?? 0
                })
                .OrderByDescending(p => p.Cantidad)
                .ToList();

            return new GlobalMetricsDto
            {
                TotalFederaciones = federaciones.Count,
                TotalClubesAfiliados = totalClubes,
                TotalAtletasGlobales = totalAtletas,
                TorneosActivosGlobales = torneosActivos,
                IngresosMensuales = ingresosMensuales,
                FederacionesFacturando = federacionesActivas.Count(f => f.PlanSaaS != null),
                PorcentajeCrecimientoAtletas = porcentajeCrecimiento,
                CrecimientoMensual = crecimiento,
                DistribucionPlanes = distribucionPlanes,
                TopFederaciones = federaciones
                    .Select(f => new FederacionMetricDto
                    {
                        Nombre = f.Nombre,
                        ClubesCount = _context.Clubes.Count(c => c.IdFederacion == f.IdFederacion)
                    })
                    .OrderByDescending(f => f.ClubesCount)
                    .Take(5)
                    .ToList()
            };
        }
    }
}
