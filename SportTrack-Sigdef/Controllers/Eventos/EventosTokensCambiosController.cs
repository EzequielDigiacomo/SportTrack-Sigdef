using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Inscripcion;
using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers.Eventos
{
    [ApiController]
    [Route("api/Eventos/{eventoId:int}/tokens-cambios")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class EventosTokensCambiosController : ControllerBase
    {
        private readonly ITokenAccesoCambiosService _tokenService;

        public EventosTokensCambiosController(ITokenAccesoCambiosService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TokenAccesoCambiosDto>>> Listar(int eventoId)
        {
            var result = await _tokenService.ListarPorEventoAsync(eventoId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<TokenAccesoCambiosDto>>> Generar(
            int eventoId,
            [FromBody] TokenAccesoCambiosCreateDto? dto)
        {
            var creadoPor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(ClaimTypes.Name)?.Value;
            var result = await _tokenService.GenerarAsync(eventoId, dto ?? new TokenAccesoCambiosCreateDto(), creadoPor);
            return Ok(result);
        }

        [HttpDelete("{tokenId:int}")]
        public async Task<IActionResult> Revocar(int eventoId, int tokenId)
        {
            await _tokenService.RevocarAsync(eventoId, tokenId);
            return NoContent();
        }
    }
}
