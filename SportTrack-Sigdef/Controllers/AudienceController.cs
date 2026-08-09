using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Audience;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,soporte_tecnico")]
    public class AudienceController : ControllerBase
    {
        private readonly IAudienceMetricsService _metrics;

        public AudienceController(IAudienceMetricsService metrics)
        {
            _metrics = metrics;
        }

        /// <summary>Conexiones SignalR actuales + % saturación vs SoftCapacity.</summary>
        [HttpGet("live")]
        public async Task<ActionResult<AudienceLiveDto>> GetLive(CancellationToken ct)
        {
            await _metrics.EnsureReadyAsync(ct);
            return Ok(_metrics.GetLive());
        }

        /// <summary>Historial / picos de audiencia concurrente.</summary>
        [HttpGet("peaks")]
        public async Task<ActionResult> GetPeaks([FromQuery] int limit = 100, CancellationToken ct = default)
        {
            var peaks = await _metrics.GetPeaksAsync(limit, ct);
            return Ok(peaks);
        }

        /// <summary>Presets de techo de control (no limitan conexiones).</summary>
        [HttpGet("capacity")]
        public async Task<ActionResult<AudienceCapacityConfigDto>> GetCapacity(CancellationToken ct)
        {
            await _metrics.EnsureReadyAsync(ct);
            return Ok(_metrics.GetCapacityConfig());
        }

        /// <summary>Cambia el techo de referencia para el % de saturación. No corta conexiones.</summary>
        [HttpPut("capacity")]
        public async Task<ActionResult<AudienceCapacityConfigDto>> PutCapacity(
            [FromBody] AudienceCapacityUpdateRequest request,
            CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new { message = "Body requerido" });

            if (string.IsNullOrWhiteSpace(request.PresetId) && request.SoftCapacity is > 0)
                request.PresetId = "custom";

            if (string.IsNullOrWhiteSpace(request.PresetId) && request.SoftCapacity is null)
                return BadRequest(new { message = "Indicá presetId o softCapacity" });

            await _metrics.ApplyCapacityAsync(request, ct);
            return Ok(_metrics.GetCapacityConfig());
        }
    }
}
