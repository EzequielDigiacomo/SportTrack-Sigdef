using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Audit;
using SportTrack_Sigdef.Controladores.Timing;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace SportTrack_Sigdef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protegido inicialmente, luego filtraremos por rol o nombre
    public class SupportController : ControllerBase
    {
        private readonly SportTrackDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ITimingOutboxService _timingOutboxService;

        public SupportController(
            SportTrackDbContext context,
            IAuditService auditService,
            ITimingOutboxService timingOutboxService)
        {
            _context = context;
            _auditService = auditService;
            _timingOutboxService = timingOutboxService;
        }

        [HttpGet("por-eventos")]
        public async Task<IActionResult> GetActividadPorEventos(
            [FromQuery] int eventosLimit = 12,
            [FromQuery] int logsPerEvento = 8)
        {
            if (!CanAccessSupportLogs()) return SupportForbidden();

            await PurgeForbidSchemeNoiseAsync();

            const string forbidNoise = "No authentication handler is registered for the scheme";
            var scoped = _context.Auditoria.Where(a =>
                !(a.Accion == "ERROR_FATAL" && a.Detalle != null && a.Detalle.Contains(forbidNoise)));

            var cards = await AuditEventCardsQuery.BuildCardsAsync(_context, scoped, eventosLimit, logsPerEvento);
            return Ok(cards);
        }

        [HttpPost("client-action")]
        public async Task<IActionResult> PostClientAction([FromBody] ClientAuditDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Accion))
                return BadRequest(new { message = "Acción requerida." });

            var accion = dto.Accion.Trim();
            if (accion.Length > 100) accion = accion[..100];
            var modulo = string.IsNullOrWhiteSpace(dto.Modulo) ? "Frontend" : dto.Modulo.Trim();
            var detalle = string.IsNullOrWhiteSpace(dto.Detalle) ? "{}" : dto.Detalle.Trim();

            await _auditService.RegistrarAccionAsync(
                accion,
                detalle,
                null,
                modulo,
                dto.EventoId,
                dto.EventoPruebaId);

            return Ok(new { ok = true });
        }

        private bool CanAccessSupportLogs()
        {
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var userName = User.Identity?.Name;
            return userRole == "SuperAdmin" || userName == "soporte_tecnico" || userName == "admin";
        }

        private IActionResult SupportForbidden() => StatusCode(StatusCodes.Status403Forbidden, new
        {
            message = "No tienes permisos para acceder a los registros de soporte."
        });

        [HttpGet("timing-outbox")]
        public async Task<IActionResult> GetTimingOutboxPending()
        {
            if (!CanAccessSupportLogs()) return SupportForbidden();

            var pending = await _timingOutboxService.GetAllPendingForSupportAsync();
            return Ok(pending);
        }

        [HttpPost("timing-outbox/{faseId:int}/commit")]
        public async Task<IActionResult> CommitTimingOutbox(int faseId, [FromBody] SupportTimingOutboxCommitDto dto)
        {
            if (!CanAccessSupportLogs()) return SupportForbidden();
            if (string.IsNullOrWhiteSpace(dto?.Username))
                return BadRequest(new { message = "Username requerido." });

            var username = dto.Username.Trim();
            var result = await _timingOutboxService.CommitAsync(username, faseId);

            if (!result.Success)
                return Conflict(result);

            await _auditService.RegistrarAccionAsync(
                "SUPPORT_TIMING_OUTBOX_COMMIT",
                $"Soporte confirmó cola temporal fase {faseId} del usuario '{username}'.",
                User.Identity?.Name,
                "Soporte");

            return Ok(result);
        }

        [HttpDelete("timing-outbox/{id:int}")]
        public async Task<IActionResult> DiscardTimingOutbox(int id)
        {
            if (!CanAccessSupportLogs()) return SupportForbidden();

            await _timingOutboxService.RemoveByIdAsync(id);

            await _auditService.RegistrarAccionAsync(
                "SUPPORT_TIMING_OUTBOX_DISCARD",
                $"Soporte descartó cola temporal Id {id}.",
                User.Identity?.Name,
                "Soporte");

            return NoContent();
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs([FromQuery] string modulo = null, [FromQuery] int limit = 100)
        {
            // Solo permitir a usuarios específicos o con el rol SuperAdmin
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var userName = User.Identity?.Name;

            if (userRole != "SuperAdmin" && userName != "soporte_tecnico" && userName != "admin")
            {
                // Forbid(string) interpreta el argumento como esquema de auth, no como mensaje.
                return SupportForbidden();
            }

            // Limpia ERROR_FATAL causados por Forbid("mensaje") mal usado (ya corregido).
            await PurgeForbidSchemeNoiseAsync();

            var query = _context.Auditoria.AsQueryable();

            if (!string.IsNullOrEmpty(modulo))
            {
                query = query.Where(a => a.Modulo == modulo);
            }

            var logs = await query
                .OrderByDescending(a => a.Fecha)
                .Take(limit)
                .ToListAsync();

            return Ok(logs);
        }

        [HttpPost("frontend-error")]
        [AllowAnonymous] // Permitir reportes incluso si no hay sesión (ej: falla el login)
        public async Task<IActionResult> PostFrontendError([FromBody] FrontendErrorDto errorDto)
        {
            var detail = new
            {
                Error = errorDto.Message,
                Url = errorDto.Url,
                Stack = errorDto.Stack,
                Browser = errorDto.BrowserInfo,
                User = User.Identity?.Name ?? "Anónimo"
            };

            await _context.Auditoria.AddAsync(new SportTrack_Sigdef.Entidades.Entidades.Auditoria
            {
                Accion = "FRONTEND_CRASH",
                Modulo = "ReactApp",
                Detalle = System.Text.Json.JsonSerializer.Serialize(detail),
                Usuario = User.Identity?.Name ?? "Anónimo",
                Fecha = DateTime.UtcNow,
                IP = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0"
            });

            await _context.SaveChangesAsync();
            return Ok();
        }

        public class FrontendErrorDto
        {
            public string Message { get; set; }
            public string Url { get; set; }
            public string Stack { get; set; }
            public string BrowserInfo { get; set; }
        }

        public class ClientAuditDto
        {
            public string Accion { get; set; } = string.Empty;
            public string? Detalle { get; set; }
            public string? Modulo { get; set; }
            public int? EventoId { get; set; }
            public int? EventoPruebaId { get; set; }
        }

        public class SupportTimingOutboxCommitDto
        {
            public string Username { get; set; } = string.Empty;
        }

        [HttpDelete("logs/clear")]
        public async Task<IActionResult> ClearLogs()
        {
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole != "SuperAdmin") return Forbid();

            // Solo borramos los de tipo ERROR para no perder auditoría legal
            var logsToRemove = _context.Auditoria.Where(a => a.Accion == "ERROR_FATAL");
            _context.Auditoria.RemoveRange(logsToRemove);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Logs de error eliminados." });
        }

        private async Task PurgeForbidSchemeNoiseAsync()
        {
            const string marker = "No authentication handler is registered for the scheme";
            var noise = await _context.Auditoria
                .Where(a => a.Accion == "ERROR_FATAL" && a.Detalle != null && a.Detalle.Contains(marker))
                .ToListAsync();

            if (noise.Count == 0) return;

            _context.Auditoria.RemoveRange(noise);
            await _context.SaveChangesAsync();
        }
    }
}

