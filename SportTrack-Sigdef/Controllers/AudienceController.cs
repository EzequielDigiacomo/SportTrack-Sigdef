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
        public ActionResult<AudienceLiveDto> GetLive()
            => Ok(_metrics.GetLive());

        /// <summary>Historial / picos de audiencia concurrente.</summary>
        [HttpGet("peaks")]
        public async Task<ActionResult> GetPeaks([FromQuery] int limit = 100, CancellationToken ct = default)
        {
            var peaks = await _metrics.GetPeaksAsync(limit, ct);
            return Ok(peaks);
        }
    }
}
