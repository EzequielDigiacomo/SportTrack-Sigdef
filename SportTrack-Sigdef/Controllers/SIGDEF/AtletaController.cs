using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGDEF.DTOs;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;
using SportTrack_Sigdef.Entidades.DTOs.Base;
using SportTrack_Sigdef.Controladores.Federaciones;
using SportTrack_Sigdef.Entidades.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGDEF.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AtletaController : ControllerBase
    {
        private readonly IAtletaServices _atletaServices;

        public AtletaController(IAtletaServices atletaServices)
        {
            _atletaServices = atletaServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetAtletas()
        {
            return await _atletaServices.GetAtletas();
        }

        [HttpGet("club/{clubId:int}")]
        public async Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetAtletasByClub(int clubId)
        {
            return await _atletaServices.GetAtletasByClub(clubId);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResponseDto<AtletaListDto>>> GetAtletasPaginados([FromQuery] PaginationParamsDto parameters)
        {
            return await _atletaServices.GetAtletasPaginadosAsync(parameters);
        }

        [HttpGet("fichajes")]
        public async Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetFichajes([FromQuery] EstadoFichaje? estado = null)
        {
            return await _atletaServices.GetFichajesPendientes(estado);
        }

        [HttpGet("padron")]
        public async Task<ActionResult<IEnumerable<PadronAtletaDto>>> GetPadron([FromQuery] PadronFiltroDto filtro)
        {
            return await _atletaServices.GetPadron(filtro);
        }

        [HttpGet("reportes/resumen")]
        public async Task<ActionResult<ReporteAdminResumenDto>> GetReporteResumen()
        {
            return await _atletaServices.GetReporteResumen();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AtletaDetailDto>> GetAtleta(int id)
        {
            return await _atletaServices.GetAtleta(id);
        }

        [HttpGet("{id:int}/fichaje/historial")]
        public async Task<ActionResult<IEnumerable<FichajeHistorialDto>>> GetHistorialFichaje(int id)
        {
            return await _atletaServices.GetHistorialFichaje(id);
        }

        [HttpPost]
        public async Task<ActionResult<AtletaDto>> PostAtleta(AtletaCreateDto atletaCreateDto)
        {
            return await _atletaServices.PostAtleta(atletaCreateDto);
        }

        [HttpPost("full")]
        public async Task<ActionResult<AtletaDto>> PostAtletaFull(AtletaFullCreateDto atletaFullCreateDto)
        {
            return await _atletaServices.PostAtletaFull(atletaFullCreateDto);
        }

        [HttpPost("{id:int}/fichaje/aprobar")]
        public async Task<IActionResult> AprobarFichaje(int id)
        {
            return await _atletaServices.AprobarFichaje(id);
        }

        [HttpPost("{id:int}/fichaje/observar")]
        public async Task<IActionResult> ObservarFichaje(int id, [FromBody] FichajeRevisionRequestDto dto)
        {
            return await _atletaServices.ObservarFichaje(id, dto ?? new FichajeRevisionRequestDto());
        }

        [HttpPost("{id:int}/fichaje/rechazar")]
        public async Task<IActionResult> RechazarFichaje(int id, [FromBody] FichajeRevisionRequestDto dto)
        {
            return await _atletaServices.RechazarFichaje(id, dto ?? new FichajeRevisionRequestDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAtleta(int id, AtletaCreateDto atletaCreateDto)
        {
            return await _atletaServices.PutAtleta(id, atletaCreateDto);
        }

        [HttpPost("{id:int}/liberar")]
        public async Task<IActionResult> LiberarAtleta(int id)
        {
            return await _atletaServices.LiberarAtleta(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAtleta(int id)
        {
            return await _atletaServices.DeleteAtleta(id);
        }
    }
}

