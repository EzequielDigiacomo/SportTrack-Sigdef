using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Audit;
using SportTrack_Sigdef.Controladores.Federaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuditoriaController : ControllerBase
    {
        private readonly SportTrackDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly IAuditService _auditService;

        public AuditoriaController(
            SportTrackDbContext context,
            ITenantProvider tenantProvider,
            IAuditService auditService)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _auditService = auditService;
        }

        /// <summary>
        /// Actividad reciente del sistema.
        /// SuperAdmin: global. Admin/Federacion: solo usuarios de su federación.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActividad([FromQuery] int limit = 100, [FromQuery] int? eventoId = null)
        {
            limit = Math.Clamp(limit, 1, 500);

            var query = BuildScopedQuery();

            if (eventoId.HasValue && eventoId.Value > 0)
            {
                query = query.Where(a => a.IdEvento == eventoId.Value);
            }

            var logs = await query
                .OrderByDescending(a => a.Fecha)
                .Take(limit)
                .Select(a => new
                {
                    id = a.Id,
                    fecha = a.Fecha,
                    accion = a.Accion,
                    detalle = a.Detalle,
                    usuario = a.Usuario,
                    modulo = a.Modulo,
                    ip = a.IP,
                    idEvento = a.IdEvento,
                    idEventoPrueba = a.IdEventoPrueba,
                })
                .ToListAsync();

            return Ok(logs);
        }

        /// <summary>
        /// Tarjetas de auditoría agrupadas por evento (actividad reciente por competencia).
        /// </summary>
        [HttpGet("por-eventos")]
        public async Task<IActionResult> GetActividadPorEventos(
            [FromQuery] int eventosLimit = 12,
            [FromQuery] int logsPerEvento = 8)
        {
            eventosLimit = Math.Clamp(eventosLimit, 1, 50);
            logsPerEvento = Math.Clamp(logsPerEvento, 1, 30);

            var scoped = BuildScopedQuery();

            var grouped = await scoped
                .Where(a => a.IdEvento != null)
                .GroupBy(a => a.IdEvento)
                .Select(g => new
                {
                    EventoId = g.Key!.Value,
                    UltimaActividad = g.Max(a => a.Fecha),
                    Total = g.Count(),
                })
                .ToListAsync();

            var legacyCandidates = await scoped
                .Where(a => a.IdEvento == null && (
                    a.Modulo == "Competencia"
                    || a.Modulo == "Inscripciones"
                    || a.Modulo == "Eventos"
                    || a.Modulo == "Frontend"
                    || a.Accion.StartsWith("CLICK_")
                    || a.Accion.StartsWith("OPEN_")))
                .OrderByDescending(a => a.Fecha)
                .Take(1500)
                .AsNoTracking()
                .ToListAsync();

            var legacyGroups = await AuditLegacyScopeResolver.GroupLegacyLogsByEventoAsync(_context, legacyCandidates);

            var groupedMap = grouped.ToDictionary(g => g.EventoId);

            foreach (var kv in legacyGroups)
            {
                var eventoId = kv.Key;
                var logs = kv.Value;
                if (logs.Count == 0) continue;

                var ultima = logs.Max(l => l.Fecha);
                if (!groupedMap.TryGetValue(eventoId, out var existing))
                {
                    groupedMap[eventoId] = new
                    {
                        EventoId = eventoId,
                        UltimaActividad = ultima,
                        Total = logs.Count,
                    };
                }
                else
                {
                    groupedMap[eventoId] = new
                    {
                        existing.EventoId,
                        UltimaActividad = existing.UltimaActividad > ultima ? existing.UltimaActividad : ultima,
                        Total = existing.Total + logs.Count,
                    };
                }
            }

            var eventoIds = groupedMap.Values
                .OrderByDescending(x => x.UltimaActividad)
                .Take(eventosLimit)
                .ToList();

            if (eventoIds.Count == 0)
            {
                return Ok(Array.Empty<object>());
            }

            var ids = eventoIds.Select(x => x.EventoId).ToList();
            var eventos = await _context.Eventos
                .AsNoTracking()
                .Where(e => ids.Contains(e.IdEvento))
                .Select(e => new { e.IdEvento, e.Nombre, e.Estado, e.Fecha })
                .ToDictionaryAsync(e => e.IdEvento);

            var cards = new List<object>();

            foreach (var row in eventoIds)
            {
                var directLogs = await scoped
                    .Where(a => a.IdEvento == row.EventoId)
                    .OrderByDescending(a => a.Fecha)
                    .Take(logsPerEvento)
                    .Select(a => new
                    {
                        id = a.Id,
                        fecha = a.Fecha,
                        accion = a.Accion,
                        detalle = a.Detalle,
                        usuario = a.Usuario,
                        modulo = a.Modulo,
                        ip = a.IP,
                        idEvento = a.IdEvento,
                        idEventoPrueba = a.IdEventoPrueba,
                    })
                    .ToListAsync();

                legacyGroups.TryGetValue(row.EventoId, out var legacyForEvent);
                var legacyMapped = (legacyForEvent ?? new List<Entidades.Entidades.Auditoria>())
                    .OrderByDescending(l => l.Fecha)
                    .Take(logsPerEvento)
                    .Select(a => new
                    {
                        id = a.Id,
                        fecha = a.Fecha,
                        accion = a.Accion,
                        detalle = a.Detalle,
                        usuario = a.Usuario,
                        modulo = a.Modulo,
                        ip = a.IP,
                        idEvento = (int?)row.EventoId,
                        idEventoPrueba = a.IdEventoPrueba,
                    });

                var logs = directLogs
                    .Concat(legacyMapped)
                    .OrderByDescending(l => l.fecha)
                    .Take(logsPerEvento)
                    .ToList();

                eventos.TryGetValue(row.EventoId, out var ev);

                cards.Add(new
                {
                    eventoId = row.EventoId,
                    eventoNombre = ev?.Nombre ?? $"Evento #{row.EventoId}",
                    eventoEstado = ev?.Estado,
                    eventoFecha = ev?.Fecha,
                    ultimaActividad = row.UltimaActividad,
                    totalRegistros = row.Total,
                    logs,
                });
            }

            return Ok(cards);
        }

        /// <summary>
        /// Acciones registradas desde el front (módulo abierto, botón apretado, etc.).
        /// </summary>
        [HttpPost("client-action")]
        public async Task<IActionResult> PostClientAction([FromBody] ClientAuditDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Accion))
            {
                return BadRequest(new { message = "Acción requerida." });
            }

            var accion = dto.Accion.Trim();
            if (accion.Length > 100) accion = accion[..100];

            var modulo = string.IsNullOrWhiteSpace(dto.Modulo) ? "Frontend" : dto.Modulo.Trim();
            var detalle = string.IsNullOrWhiteSpace(dto.Detalle)
                ? "{}"
                : dto.Detalle.Trim();

            await _auditService.RegistrarAccionAsync(
                accion,
                detalle,
                null,
                modulo,
                dto.EventoId,
                dto.EventoPruebaId);

            return Ok(new { ok = true });
        }

        public class ClientAuditDto
        {
            public string Accion { get; set; } = string.Empty;
            public string? Detalle { get; set; }
            public string? Modulo { get; set; }
            public int? EventoId { get; set; }
            public int? EventoPruebaId { get; set; }
        }

        private IQueryable<Entidades.Entidades.Auditoria> BuildScopedQuery()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value
                ?? User.FindFirst("role")?.Value
                ?? string.Empty;

            var isSuperAdmin = string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(role, "soporte_tecnico", StringComparison.OrdinalIgnoreCase)
                || string.Equals(User.Identity?.Name, "admin", StringComparison.OrdinalIgnoreCase);

            var query = _context.Auditoria.AsQueryable();

            const string forbidNoise = "No authentication handler is registered for the scheme";
            query = query.Where(a =>
                !(a.Accion == "ERROR_FATAL" && a.Detalle != null && a.Detalle.Contains(forbidNoise)));

            if (isSuperAdmin)
            {
                return query;
            }

            var fedId = _tenantProvider.GetFederacionId();
            if (!fedId.HasValue || fedId.Value <= 0)
            {
                return query.Where(_ => false);
            }

            var clubIds = _context.Clubes
                .AsNoTracking()
                .Where(c => c.IdFederacion == fedId.Value)
                .Select(c => c.IdClub);

            var usernames = _context.Usuarios
                .AsNoTracking()
                .Where(u =>
                    u.IdFederacion == fedId.Value
                    || (u.IdClub.HasValue && clubIds.Contains(u.IdClub.Value)))
                .Select(u => u.Username)
                .Distinct();

            return query.Where(a => usernames.Contains(a.Usuario));
        }
    }
}
