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
            return planes
                .OrderBy(PlanSaaSAccessHelper.CatalogSortKey)
                .ThenBy(p => p.Id)
                .Select(PlanSaaSAccessHelper.FromEntity);
        }

        public async Task<PlanSaaSDto> GetPlanByIdAsync(int id)
        {
            var p = await _context.PlanesSaaS.FindAsync(id);
            if (p == null) return null;

            return PlanSaaSAccessHelper.FromEntity(p);
        }

        public async Task<PlanSaaSDto?> UpdatePlanAsync(int id, PlanSaaSUpdateDto dto)
        {
            var plan = await _context.PlanesSaaS.FindAsync(id);
            if (plan == null) return null;

            var precioAnterior = plan.Precio;
            var descuentoAnterior = plan.DescuentoAnualPorcentaje;
            var atletasAnterior = plan.MaxAtletas;

            plan.Precio = dto.Precio;
            plan.DescuentoAnualPorcentaje = dto.DescuentoAnualPorcentaje;
            plan.PrecioAnual = PlanSaaSAccessHelper.CalcularPrecioAnual(dto.Precio, dto.DescuentoAnualPorcentaje);
            plan.MaxAtletas = dto.MaxAtletas;
            // Torneos: sin límite comercial
            plan.MaxTorneosActivos = -1;

            await _context.SaveChangesAsync();

            if (precioAnterior != plan.Precio || descuentoAnterior != plan.DescuentoAnualPorcentaje || atletasAnterior != plan.MaxAtletas)
            {
                await _auditService.RegistrarAccionAsync(
                    "UPDATE_PLAN",
                    $"Plan '{plan.Nombre}' actualizado: mensual {precioAnterior}→{plan.Precio}, desc. anual {descuentoAnterior}%→{plan.DescuentoAnualPorcentaje}% (anual {plan.PrecioAnual}), máx. atletas {atletasAnterior}→{plan.MaxAtletas}.",
                    modulo: "SaaS"
                );
            }

            return PlanSaaSAccessHelper.FromEntity(plan);
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
                // Torneos sin límite comercial (-1)
                const int maxTorneos = -1;

                var atletasRegistrados = c.Clubes.Sum(a => a.Participantes.Count);
                var usuariosCount = c.Usuarios.Count + c.Clubes.Sum(a => a.Usuarios.Count);
                
                var torneosDetalle = eventosActivos
                    .Where(e => e.IdFederacion == c.IdFederacion)
                    .Select(e => new TorneoSaaSDetailDto { Id = e.Id, Nombre = e.Nombre, Fecha = e.Fecha, Estado = e.Estado })
                    .ToList();
                
                var torneosActivosCount = torneosDetalle.Count;

                var alDia = true;
                if (maxAtletas != -1 && atletasRegistrados > maxAtletas) alDia = false;
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
            // Sin BeginTransaction manual: EnableRetryOnFailure lo rechaza.
            // Un solo SaveChanges es atómico (EF abre la tx interna) y compatible con reintentos.
            var nombre = (dto.Nombre ?? string.Empty).Trim();
            var username = (dto.AdminUsername ?? string.Empty).Trim().ToLower();
            var adminEmail = (dto.AdminEmail ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre de la federación es obligatorio.");
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("El usuario administrador es obligatorio.");
            if (string.IsNullOrWhiteSpace(adminEmail))
                throw new Exception("El email del administrador es obligatorio.");

            // Validación previa: evita el mensaje genérico de "duplicate key" y reintentos inútiles
            if (await _context.Federaciones.AnyAsync(f => f.Nombre.ToLower() == nombre.ToLower()))
                throw new Exception($"Ya existe una federación con el nombre '{nombre}'. Revisá el listado: puede haberse creado en intentos anteriores.");

            if (await _context.Usuarios.AnyAsync(u => u.Username == username))
                throw new Exception($"El usuario '{username}' ya está en uso. Elegí otro o eliminá la federación duplicada que lo creó.");

            if (await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == adminEmail.ToLower()))
                throw new Exception($"El email '{adminEmail}' ya está registrado. Elegí otro o eliminá la federación duplicada que lo usó.");

            try
            {
                var fed = new Entidades.Entidades.Federacion
                {
                    Nombre = nombre,
                    Sigla = dto.Sigla,
                    Email = dto.Email,
                    Telefono = dto.Telefono ?? string.Empty,
                    Direccion = dto.Direccion ?? string.Empty,
                    Cuit = string.Empty,
                    Activo = true,
                    PlanSaaSId = 1,
                    FechaAltaPlan = DateTime.UtcNow.Date,
                    FechaVencimientoPlan = DateTime.UtcNow.Date.AddMonths(1)
                };

                var user = new Entidades.Entidades.Usuario
                {
                    Username = username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.AdminPassword),
                    Email = adminEmail,
                    RolFederacion = "Admin",
                    Federacion = fed,
                    EstaActivo = true
                };

                _context.Federaciones.Add(fed);
                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();

                // Asegurar FK explícita por si el tracking no propagó IdFederacion al DTO de listados
                if (user.IdFederacion != fed.IdFederacion)
                {
                    user.IdFederacion = fed.IdFederacion;
                    await _context.SaveChangesAsync();
                }

                await _auditService.RegistrarAccionAsync(
                    "CREATE_FEDERATION",
                    $"Federación '{fed.Nombre}' (Id={fed.IdFederacion}) creada con admin '{username}'.",
                    modulo: "SaaS"
                );

                return fed.IdFederacion;
            }
            catch (Exception ex) when (ex.Message.StartsWith("Ya existe") || ex.Message.StartsWith("El usuario") || ex.Message.StartsWith("El email") || ex.Message.StartsWith("El nombre"))
            {
                throw;
            }
            catch (Exception ex)
            {
                foreach (var entry in _context.ChangeTracker.Entries().ToList())
                    entry.State = EntityState.Detached;

                var innerMsg = ex.InnerException?.Message ?? ex.Message;

                if (innerMsg.Contains("23505") || innerMsg.Contains("duplicate key", StringComparison.OrdinalIgnoreCase))
                {
                    if (innerMsg.Contains("IX_Usuarios_Username", StringComparison.OrdinalIgnoreCase) || innerMsg.Contains("Username", StringComparison.OrdinalIgnoreCase))
                        throw new Exception("El nombre de usuario administrador ya está en uso. Por favor, elige otro.");
                    if (innerMsg.Contains("IX_Usuarios_Email", StringComparison.OrdinalIgnoreCase) || innerMsg.Contains("Email", StringComparison.OrdinalIgnoreCase))
                        throw new Exception("El email del administrador ya está registrado en otra cuenta. Debe ser único.");
                    if (innerMsg.Contains("IX_Federaciones_Nombre", StringComparison.OrdinalIgnoreCase) || innerMsg.Contains("IX_Clubes_Nombre", StringComparison.OrdinalIgnoreCase))
                        throw new Exception("Ya existe una federación o club con ese nombre.");

                    throw new Exception($"Un dato ya existe en el sistema (posible duplicado por reintentos). Detalle: {innerMsg}");
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
