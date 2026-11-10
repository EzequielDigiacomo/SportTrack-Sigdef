using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Inscripcion;
using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers.Inscripciones
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InscripcionesController : ControllerBase
    {
        private readonly IInscripcionService _inscripcionService;
        private readonly IInscripcionLiveOpsService _liveOpsService;

        public InscripcionesController(
            IInscripcionService inscripcionService,
            IInscripcionLiveOpsService liveOpsService)
        {
            _inscripcionService = inscripcionService;
            _liveOpsService = liveOpsService;
        }

        private bool IsAdminRole()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase);
        }

        private static int? ParseClaimId(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return int.TryParse(value, out var id) && id > 0 ? id : null;
        }

        [HttpGet("registro")]
        public async Task<ActionResult<IEnumerable<RegistroInscripcionDto>>> GetRegistro(
            [FromQuery] int? eventoId,
            [FromQuery] int? clubId,
            [FromQuery] int? participanteId,
            [FromQuery] string? busqueda)
        {
            var userClubId = ParseClaimId(User.FindFirst("ClubId")?.Value);
            var federacionId = ParseClaimId(User.FindFirst("FederacionId")?.Value);
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            int? clubScope = null;
            int? federacionScope = null;

            if (string.Equals(role, "Club", StringComparison.OrdinalIgnoreCase))
            {
                clubScope = userClubId;
            }
            else if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                federacionScope = federacionId;
            }

            var result = await _inscripcionService.GetRegistroInscripcionesAsync(
                clubScope,
                federacionScope,
                eventoId,
                clubScope.HasValue ? null : clubId,
                participanteId,
                busqueda);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripciones()
        {
            var result = await _inscripcionService.GetAllInscripcionesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InscripcionDto>> GetInscripcion(int id)
        {
            var result = await _inscripcionService.GetInscripcionByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize] // Solo usuarios registrados (Clubes o Admin) pueden inscribir
        public async Task<ActionResult<InscripcionDto>> CreateInscripcion(InscripcionCreateDto inscripcionDto)
        {
            // Opcional: Validar que el clubId del usuario coincida con el club del participante
            // var clubId = User.FindFirst("ClubId")?.Value;
            
            var result = await _inscripcionService.CreateInscripcionAsync(inscripcionDto);
            return CreatedAtAction(nameof(GetInscripcion), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<InscripcionDto>> UpdateInscripcion(int id, InscripcionUpdateDto inscripcionDto)
        {
            var result = await _inscripcionService.UpdateInscripcionAsync(id, inscripcionDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteInscripcion(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var allowWhenClosed = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

            await _inscripcionService.DeleteInscripcionAsync(id, allowWhenClosed);
            return NoContent();
        }

        [HttpGet("evento-prueba/{eventoPruebaId}")]
        public async Task<ActionResult<IEnumerable<InscripcionDto>>> GetByEventoPrueba(int eventoPruebaId)
        {
            var result = await _inscripcionService.GetInscripcionesByEventoPruebaIdAsync(eventoPruebaId);
            return Ok(result);
        }

        [HttpGet("evento/{eventoId}/club/{clubId}")]
        public async Task<ActionResult<IEnumerable<InscripcionDto>>> GetByEventoAndClub(int eventoId, int clubId)
        {
            var result = await _inscripcionService.GetInscripcionesByEventoAndClubAsync(eventoId, clubId);
            return Ok(result);
        }

        [HttpPatch("{id}/toggle-seeding")]
        [Authorize]
        public async Task<ActionResult> ToggleSeeding(int id)
        {
            await _inscripcionService.ToggleEsCabezaDeSerieAsync(id);
            return Ok();
        }

        /// <summary>Corrección de tripulación pre-delegados (Club) o admin.</summary>
        [HttpPatch("{id}/tripulacion")]
        [Authorize]
        public async Task<ActionResult<InscripcionDto>> UpdateTripulacion(int id, InscripcionTripulacionUpdateDto dto)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var isAdmin = IsAdminRole();
            int? clubScope = null;
            if (string.Equals(role, "Club", StringComparison.OrdinalIgnoreCase))
                clubScope = ParseClaimId(User.FindFirst("ClubId")?.Value);

            var result = await _inscripcionService.UpdateTripulacionAsync(
                id,
                dto,
                clubScope,
                bypassDeadline: isAdmin,
                requireSameClub: true);
            return Ok(result);
        }

        // ===== Live ops (solo Admin / SuperAdmin) =====

        [HttpPatch("live/{id}/tripulacion")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<InscripcionDto>> LiveUpdateTripulacion(int id, InscripcionTripulacionUpdateDto dto)
        {
            var result = await _liveOpsService.UpdateTripulacionAdminAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("live/participantes/{participanteId}/nombre")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> LivePatchNombre(int participanteId, ParticipanteNombrePatchDto dto)
        {
            await _liveOpsService.PatchParticipanteNombreAsync(participanteId, dto);
            return NoContent();
        }

        [HttpPatch("live/{id}/numero")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<InscripcionDto>> LiveUpdateNumero(int id, [FromBody] InscripcionUpdateDto dto)
        {
            var result = await _liveOpsService.UpdateNumeroCompetidorAsync(id, dto.NumeroCompetidor ?? string.Empty);
            return Ok(result);
        }

        [HttpPut("live/fases/{faseId}/carriles")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> LiveReasignarCarriles(int faseId, LiveCarrilesBatchDto dto)
        {
            await _liveOpsService.ReasignarCarrilesAsync(faseId, dto);
            return NoContent();
        }

        [HttpDelete("live/{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> LiveBaja(int id, [FromQuery] bool force = false)
        {
            await _liveOpsService.BajaBoteAsync(id, force);
            return NoContent();
        }

        [HttpPost("live/agregar")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<InscripcionDto>> LiveAgregar(LiveAgregarBoteDto dto)
        {
            var result = await _liveOpsService.AgregarBoteAsync(dto);
            return Ok(result);
        }

        [HttpPost("live/{id}/mover-prueba")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<InscripcionDto>> LiveMover(int id, LiveMoverPruebaDto dto)
        {
            var result = await _liveOpsService.MoverPruebaAsync(id, dto);
            return Ok(result);
        }
    }
}

