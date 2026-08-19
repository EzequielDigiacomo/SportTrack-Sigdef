using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Auth.Dtos;
using SportTrack_Sigdef.Controladores.Evento;
using SportTrack_Sigdef.Controladores.Evento.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.Fase;
using SportTrack_Sigdef.Controladores.Fase.Dtos;

namespace SportTrack_Sigdef.Controllers.Eventos
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IFaseService _faseService;
        private readonly SportTrack_Sigdef.Controladores.Auth.IAuthService _authService;

        public EventosController(
            IEventoService eventoService, 
            IFaseService faseService,
            SportTrack_Sigdef.Controladores.Auth.IAuthService authService)
        {
            _eventoService = eventoService;
            _faseService = faseService;
            _authService = authService;
        }

        private static int? ParseClaimId(string? value) =>
            int.TryParse(value, out var id) && id > 0 ? id : null;

        private async Task<TenantScopeHelper.EventListScope> ResolveEventScopeAsync(
            int? queryClubId = null,
            int? queryFederacionId = null)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst(ClaimTypes.Name)?.Value;

            if (!string.IsNullOrEmpty(username))
            {
                try
                {
                    var userDb = await _authService.GetMeAsync(username);
                    return TenantScopeHelper.ResolveEventListScope(userDb, queryClubId, queryFederacionId);
                }
                catch
                {
                    var role = User.FindFirst(ClaimTypes.Role)?.Value
                               ?? User.FindFirst("role")?.Value
                               ?? User.FindFirst("Rol")?.Value
                               ?? string.Empty;

                    return TenantScopeHelper.ResolveEventListScopeFromClaims(
                        role,
                        ParseClaimId(User.FindFirst("ClubId")?.Value),
                        ParseClaimId(User.FindFirst("FederacionId")?.Value),
                        queryClubId,
                        queryFederacionId);
                }
            }

            return new TenantScopeHelper.EventListScope(string.Empty, null, null);
        }

        private async Task<bool> CanAccessEventoAsync(int eventoId, TenantScopeHelper.EventListScope scope)
        {
            if (TenantScopeHelper.IsSuperAdmin(scope.Role))
                return true;

            if (scope.FederacionId is > 0)
                return await _eventoService.EventoBelongsToFederationAsync(eventoId, scope.FederacionId.Value);

            if (scope.ClubId is > 0)
            {
                var evento = await _eventoService.GetEventoByIdAsync(eventoId);
                return evento.ClubId == scope.ClubId;
            }

            return false;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventoDto>>> GetEventos(
            [FromQuery] int? clubId = null,
            [FromQuery] int? federacionId = null)
        {
            var scope = await ResolveEventScopeAsync(clubId, federacionId);
            var result = await _eventoService.GetAllEventosAsync(scope.ClubId, scope.Role, scope.FederacionId);
            return Ok(result);
        }

        [HttpGet("debug")]
        public async Task<ActionResult> DebugEvents()
        {
            var scope = await ResolveEventScopeAsync();
            var result = await _eventoService.GetAllEventosAsync(scope.ClubId, scope.Role, scope.FederacionId);
            return Ok(new {
                Role = scope.Role,
                RoleLength = scope.Role?.Length,
                ClubId = scope.ClubId,
                FederacionId = scope.FederacionId,
                EventsCount = result.Count()
            });
        }

        [HttpGet("{id}/fases")]
        [AllowAnonymous]
        [EnableRateLimiting("live")]
        public async Task<ActionResult<IEnumerable<FaseDto>>> GetFases(int id)
        {
            var result = await _faseService.GetFasesPorEventoAsync(id);
            return Ok(result);
        }

        [HttpGet("proximos")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EventoDto>>> GetProximosEventos(
            [FromQuery] int? clubId = null,
            [FromQuery] int? federacionId = null)
        {
            TenantScopeHelper.EventListScope scope;

            if (User.Identity?.IsAuthenticated == true)
            {
                scope = await ResolveEventScopeAsync(clubId, federacionId);
            }
            else
            {
                scope = new TenantScopeHelper.EventListScope(string.Empty, clubId, federacionId);
            }

            var result = await _eventoService.GetProximosEventosAsync(scope.ClubId, scope.Role, scope.FederacionId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [EnableRateLimiting("live")]
        public async Task<ActionResult<EventoDto>> GetEvento(int id)
        {
            var result = await _eventoService.GetEventoByIdAsync(id);

            if (User.Identity?.IsAuthenticated == true)
            {
                var scope = await ResolveEventScopeAsync();
                if (!TenantScopeHelper.IsSuperAdmin(scope.Role))
                {
                    if (!await CanAccessEventoAsync(id, scope))
                        return NotFound(new { message = "Evento no encontrado." });
                }
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<EventoDto>> CreateEvento(EventoCreateDto eventoDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst(ClaimTypes.Name)?.Value;

            if (!string.IsNullOrEmpty(username))
            {
                try
                {
                    var userDb = await _authService.GetMeAsync(username);
                    if (userDb.FederacionId.HasValue && userDb.FederacionId.Value > 0)
                    {
                        eventoDto.FederacionId = userDb.FederacionId;
                    }
                    if (userDb.ClubId.HasValue && userDb.ClubId.Value > 0)
                    {
                        eventoDto.ClubId = userDb.ClubId;
                    }
                }
                catch
                {
                    var role = User.FindFirst(ClaimTypes.Role)?.Value;
                    if (role == "Club" || role == "Admin")
                    {
                        var clubIdClaim = User.FindFirst("ClubId")?.Value;
                        if (int.TryParse(clubIdClaim, out int cid) && cid > 0) eventoDto.ClubId = cid;

                        var fedIdClaim = User.FindFirst("FederacionId")?.Value;
                        if (int.TryParse(fedIdClaim, out int fedId) && fedId > 0) eventoDto.FederacionId = fedId;
                    }
                }
            }

            var result = await _eventoService.CreateEventoAsync(eventoDto);
            return CreatedAtAction(nameof(GetEvento), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EventoDto>> UpdateEvento(int id, EventoUpdateDto eventoDto)
        {
            int? clubId = null;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Club" || role == "Admin")
            {
                var clubIdClaim = User.FindFirst("ClubId")?.Value;
                if (int.TryParse(clubIdClaim, out int cid) && cid > 0) clubId = cid;
            }

            var result = await _eventoService.UpdateEventoAsync(id, eventoDto, clubId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            int? clubId = null;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Club" || role == "Admin")
            {
                var clubIdClaim = User.FindFirst("ClubId")?.Value;
                if (int.TryParse(clubIdClaim, out int cid) && cid > 0) clubId = cid;
            }

            await _eventoService.DeleteEventoAsync(id, clubId);
            return NoContent();
        }

        [HttpGet("{id}/pruebas")]
        [AllowAnonymous]
        [EnableRateLimiting("live")]
        public async Task<ActionResult<IEnumerable<EventoPruebaDto>>> GetPruebas(int id)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var scope = await ResolveEventScopeAsync();
                if (!TenantScopeHelper.IsSuperAdmin(scope.Role))
                {
                    if (!await CanAccessEventoAsync(id, scope))
                        return NotFound(new { message = "Evento no encontrado." });
                }
            }

            var result = await _eventoService.GetPruebasByEventoAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/pruebas")]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EventoPruebaDto>> AssignPrueba(int id, EventoPruebaCreateDto assignDto)
        {
            var result = await _eventoService.AssignPruebaToEventoAsync(id, assignDto);
            return Ok(result);
        }

        /// <summary>Largada Maratón: varias categorías/botes/sexos en el mismo pateo.</summary>
        [HttpPost("{id}/pruebas/largada")]
        public async Task<ActionResult<IEnumerable<EventoPruebaDto>>> AssignLargada(int id, EventoLargadaCreateDto largadaDto)
        {
            try
            {
                var result = await _eventoService.AssignLargadaMaratonAsync(id, largadaDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("pruebas/{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EventoPruebaDto>> UpdatePrueba(int id, EventoPruebaCreateDto updateDto)
        {
            var result = await _eventoService.UpdateEventoPruebaAsync(id, updateDto);
            return Ok(result);
        }

        [HttpDelete("pruebas/{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnassignPrueba(int id)
        {
            await _eventoService.DeleteEventoPruebaAsync(id);
            return NoContent();
        }
    }
}
