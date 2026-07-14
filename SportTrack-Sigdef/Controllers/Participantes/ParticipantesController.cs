using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Participante;
using SportTrack_Sigdef.Controladores.Participante.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers.Participantes
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParticipantesController : ControllerBase
    {
        private readonly IParticipanteService _participanteService;

        public ParticipantesController(IParticipanteService participanteService)
        {
            _participanteService = participanteService;
        }

        private static int? ParseClaimId(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return int.TryParse(value, out var id) && id > 0 ? id : null;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipanteDto>>> GetParticipantes()
        {
            var clubId = ParseClaimId(User.FindFirst("ClubId")?.Value);
            var federacionId = ParseClaimId(User.FindFirst("FederacionId")?.Value);
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _participanteService.GetAllParticipantesAsync(clubId, roleClaim, federacionId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipanteDto>> GetParticipante(int id)
        {
            var result = await _participanteService.GetParticipanteByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("club/{clubId}")]
        public async Task<ActionResult<IEnumerable<ParticipanteDto>>> GetByClub(int clubId)
        {
            var result = await _participanteService.GetParticipantesByClubAsync(clubId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ParticipanteDto>> CreateParticipante(ParticipanteCreateDto participanteDto)
        {
            var result = await _participanteService.CreateParticipanteAsync(participanteDto);
            return CreatedAtAction(nameof(GetParticipante), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ParticipanteDto>> UpdateParticipante(int id, ParticipanteCreateDto participanteDto)
        {
            var result = await _participanteService.UpdateParticipanteAsync(id, participanteDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteParticipante(int id)
        {
            await _participanteService.DeleteParticipanteAsync(id);
            return NoContent();
        }
    }
}
