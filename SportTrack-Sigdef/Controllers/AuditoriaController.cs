using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Federaciones;
using System;
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

        public AuditoriaController(SportTrackDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        /// <summary>
        /// Actividad reciente del sistema.
        /// SuperAdmin: global. Admin/Federacion: solo usuarios de su federación.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActividad([FromQuery] int limit = 100)
        {
            limit = Math.Clamp(limit, 1, 500);

            var role = User.FindFirst(ClaimTypes.Role)?.Value
                ?? User.FindFirst("role")?.Value
                ?? string.Empty;

            var isSuperAdmin = string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(role, "soporte_tecnico", StringComparison.OrdinalIgnoreCase);

            var query = _context.Auditoria.AsQueryable();

            if (!isSuperAdmin)
            {
                var fedId = _tenantProvider.GetFederacionId();
                if (!fedId.HasValue || fedId.Value <= 0)
                {
                    return Forbid();
                }

                var clubIds = await _context.Clubes
                    .AsNoTracking()
                    .Where(c => c.IdFederacion == fedId.Value)
                    .Select(c => c.IdClub)
                    .ToListAsync();

                var usernames = await _context.Usuarios
                    .AsNoTracking()
                    .Where(u =>
                        u.IdFederacion == fedId.Value
                        || (u.IdClub.HasValue && clubIds.Contains(u.IdClub.Value)))
                    .Select(u => u.Username)
                    .Distinct()
                    .ToListAsync();

                if (usernames.Count == 0)
                {
                    return Ok(Array.Empty<object>());
                }

                query = query.Where(a => usernames.Contains(a.Usuario));
            }

            var logs = await query
                .AsNoTracking()
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
                    ip = a.IP
                })
                .ToListAsync();

            return Ok(logs);
        }
    }
}
