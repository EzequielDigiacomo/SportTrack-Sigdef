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
            logsPerEvento = Math.Clamp(logsPerEvento, 1, 150);

            var scoped = BuildScopedQuery();
            var cards = await AuditEventCardsQuery.BuildCardsAsync(_context, scoped, eventosLimit, logsPerEvento);
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

        /// <summary>
        /// Elimina un registro de auditoría (SuperAdmin / soporte).
        /// </summary>
        [HttpDelete("por-evento/{eventoId:int}/sin-problemas")]
        public async Task<IActionResult> DeleteRegistrosSinProblemas(int eventoId)
        {
            if (!IsSuperAdmin()) return Forbid();

            if (eventoId <= 0) return BadRequest(new { message = "Evento inválido." });

            var eventoMarker = $"\"eventoId\":{eventoId}";
            var eventoMarkerSpaced = $"\"eventoId\": {eventoId}";

            var directMatches = await _context.Auditoria
                .Where(a => a.Detalle.Contains(eventoMarker) || a.Detalle.Contains(eventoMarkerSpaced))
                .ToListAsync();

            var scoped = BuildScopedQuery();
            var legacyPool = await scoped
                .AsNoTracking()
                .OrderByDescending(a => a.Fecha)
                .Take(5000)
                .ToListAsync();

            var legacyGroups = await AuditLegacyScopeResolver.GroupLegacyLogsByEventoAsync(_context, legacyPool);
            legacyGroups.TryGetValue(eventoId, out var legacyForEvent);

            var candidates = directMatches
                .Concat(legacyForEvent ?? new List<Entidades.Entidades.Auditoria>())
                .GroupBy(a => a.Id)
                .Select(g => g.First())
                .Where(a => !AuditProblemActions.ShouldKeepOnBulkCleanup(a.Accion))
                .ToList();

            if (candidates.Count == 0)
            {
                return Ok(new { deleted = 0, message = "No hay registros OK para eliminar en este evento." });
            }

            _context.Auditoria.RemoveRange(candidates);
            await _context.SaveChangesAsync();

            return Ok(new { deleted = candidates.Count, message = $"{candidates.Count} registro(s) eliminado(s)." });
        }

        /// <summary>
        /// Elimina un registro de auditoría por Id (SuperAdmin / soporte).
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRegistro(int id)
        {
            if (!IsSuperAdmin()) return Forbid();

            var row = await _context.Auditoria.FindAsync(id);
            if (row == null) return NotFound(new { message = "Registro no encontrado." });

            _context.Auditoria.Remove(row);
            await _context.SaveChangesAsync();

            return Ok(new { ok = true, deleted = id });
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

        private bool IsSuperAdmin()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value
                ?? User.FindFirst("role")?.Value
                ?? string.Empty;

            return string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(role, "soporte_tecnico", StringComparison.OrdinalIgnoreCase)
                || string.Equals(User.Identity?.Name, "admin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(User.Identity?.Name, "soporte_tecnico", StringComparison.OrdinalIgnoreCase);
        }
    }
}
