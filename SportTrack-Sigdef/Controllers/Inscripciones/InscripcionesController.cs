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

        public InscripcionesController(IInscripcionService inscripcionService)
        {
            _inscripcionService = inscripcionService;
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
            await _inscripcionService.DeleteInscripcionAsync(id);
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
    }
}

