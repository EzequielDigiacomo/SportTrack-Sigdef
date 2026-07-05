using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;

namespace SportTrack_Sigdef.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        private readonly SportTrackDbContext _context;

        public HealthController(SportTrackDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() => Ok(new { status = "ok" });

        [HttpGet("db")]
        public async Task<IActionResult> Database()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    return StatusCode(503, new { connected = false, error = "No se pudo conectar a PostgreSQL." });
                }

                var applied = (await _context.Database.GetAppliedMigrationsAsync()).ToList();
                var pending = (await _context.Database.GetPendingMigrationsAsync()).ToList();

                return Ok(new
                {
                    connected = true,
                    appliedMigrations = applied.Count,
                    pendingMigrations = pending.Count,
                    pending = pending
                });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new
                {
                    connected = false,
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }
    }
}
