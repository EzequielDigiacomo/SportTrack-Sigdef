using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Timing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers
{
    [ApiController]
    [Route("api/timing-outbox")]
    [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
    public class TimingOutboxController : ControllerBase
    {
        private readonly ITimingOutboxService _outboxService;

        public TimingOutboxController(ITimingOutboxService outboxService)
        {
            _outboxService = outboxService;
        }

        [HttpPost]
        public async Task<ActionResult<TimingOutboxDto>> Upsert([FromBody] TimingOutboxUpsertDto dto)
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var saved = await _outboxService.UpsertAsync(username, dto);
            return Ok(saved);
        }

        [HttpGet("pending")]
        public async Task<ActionResult> GetPending()
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var pending = await _outboxService.GetPendingAsync(username);
            return Ok(pending);
        }

        [HttpPost("flush")]
        public async Task<ActionResult> FlushPending()
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var results = await _outboxService.FlushPendingAsync(username);
            return Ok(results);
        }

        [HttpPost("{faseId:int}/commit")]
        public async Task<ActionResult<TimingOutboxCommitResultDto>> Commit(int faseId)
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var result = await _outboxService.CommitAsync(username, faseId);
            if (!result.Success) return Conflict(result);
            return Ok(result);
        }

        [HttpDelete("{faseId:int}")]
        public async Task<IActionResult> Remove(int faseId)
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            await _outboxService.RemoveAsync(username, faseId);
            return NoContent();
        }

        private string? GetUsername() =>
            User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.Identity?.Name;
    }
}
