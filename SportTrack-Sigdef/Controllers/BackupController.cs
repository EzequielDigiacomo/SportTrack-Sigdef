using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.DbBackup;

namespace SportTrack_Sigdef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,soporte_tecnico")]
    public class BackupController : ControllerBase
    {
        private readonly IBackupService _backupService;

        public BackupController(IBackupService backupService)
        {
            _backupService = backupService;
        }

        /// <summary>
        /// Descarga un backup SQL.
        /// scope=full (default) | federacion&amp;idFederacion=N
        /// </summary>
        [HttpGet("download")]
        [RequestSizeLimit(512_000_000)]
        public async Task<IActionResult> Download(
            [FromQuery] string scope = "full",
            [FromQuery] int? idFederacion = null,
            CancellationToken ct = default)
        {
            try
            {
                var clientApp = Request.Headers["X-Client-App"].ToString();
                var result = await _backupService.CreateBackupAsync(scope, idFederacion, clientApp, ct);
                return File(result.Content, result.ContentType, result.FileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al generar backup: {ex.Message}" });
            }
        }

        /// <summary>Historial de backups generados (ambos sistemas, misma BD).</summary>
        [HttpGet("history")]
        public async Task<IActionResult> History([FromQuery] int limit = 50, CancellationToken ct = default)
        {
            var items = await _backupService.GetHistoryAsync(limit, ct);
            return Ok(items);
        }
    }
}
