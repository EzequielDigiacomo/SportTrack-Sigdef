using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Inscripcion;
using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers.Inscripciones
{
    /// <summary>Acceso público vía link mágico para corregir tripulaciones.</summary>
    [ApiController]
    [Route("api/cambios-acceso")]
    [AllowAnonymous]
    public class CambiosAccesoController : ControllerBase
    {
        private readonly ITokenAccesoCambiosService _tokenService;

        public CambiosAccesoController(ITokenAccesoCambiosService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("{token}")]
        public async Task<ActionResult<CambiosAccesoResumenDto>> GetResumen(string token)
        {
            var result = await _tokenService.GetResumenByTokenAsync(token);
            return Ok(result);
        }

        [HttpPatch("{token}/inscripciones/{id}/tripulacion")]
        public async Task<ActionResult<InscripcionDto>> UpdateTripulacion(
            string token,
            int id,
            InscripcionTripulacionUpdateDto dto)
        {
            var result = await _tokenService.UpdateTripulacionByTokenAsync(token, id, dto);
            return Ok(result);
        }
    }
}
