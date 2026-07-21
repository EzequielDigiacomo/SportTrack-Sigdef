using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Federaciones;
using SportTrack_Sigdef.Entidades.DTOs.Traspaso;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGDEF.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TraspasoController : ControllerBase
    {
        private readonly ITraspasoService _traspasoService;

        public TraspasoController(ITraspasoService traspasoService)
        {
            _traspasoService = traspasoService;
        }

        [HttpGet("periodos")]
        public async Task<ActionResult<IEnumerable<PeriodoTraspasoDto>>> GetPeriodos()
        {
            return Ok(await _traspasoService.GetPeriodosAsync());
        }

        [HttpGet("periodo-activo")]
        public async Task<ActionResult<PeriodoTraspasoDto?>> GetPeriodoActivo()
        {
            return Ok(await _traspasoService.GetPeriodoActivoAsync());
        }

        [HttpPost("periodos")]
        public async Task<ActionResult<PeriodoTraspasoDto>> CreatePeriodo(PeriodoTraspasoCreateDto dto)
        {
            var created = await _traspasoService.CreatePeriodoAsync(dto);
            return CreatedAtAction(nameof(GetPeriodos), new { id = created.Id }, created);
        }

        [HttpPut("periodos/{id}")]
        public async Task<ActionResult<PeriodoTraspasoDto>> UpdatePeriodo(int id, PeriodoTraspasoUpdateDto dto)
        {
            return Ok(await _traspasoService.UpdatePeriodoAsync(id, dto));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudTraspasoDto>>> GetSolicitudes([FromQuery] string? estado = null)
        {
            return Ok(await _traspasoService.GetSolicitudesAsync(estado));
        }

        [HttpGet("auditoria")]
        public async Task<ActionResult<IEnumerable<TraspasoAuditoriaDto>>> GetAuditoria([FromQuery] int limit = 50)
        {
            return Ok(await _traspasoService.GetAuditoriaAsync(limit));
        }

        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportCsv([FromQuery] int? periodoId = null, [FromQuery] string? estado = null)
        {
            var bytes = await _traspasoService.ExportSolicitudesCsvAsync(periodoId, estado);
            var fileName = $"traspasos-{DateTime.UtcNow:yyyyMMdd-HHmm}.csv";
            return File(bytes, "text/csv; charset=utf-8", fileName);
        }

        [HttpGet("buscar-atletas")]
        public async Task<ActionResult<IEnumerable<AtletaTraspasoBusquedaDto>>> BuscarAtletas([FromQuery] string term)
        {
            return Ok(await _traspasoService.BuscarAtletasAsync(term));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudTraspasoDto>> GetSolicitud(int id)
        {
            return Ok(await _traspasoService.GetSolicitudByIdAsync(id));
        }

        [HttpGet("{id}/validaciones")]
        public async Task<ActionResult<TraspasoValidacionDto>> GetValidaciones(int id)
        {
            return Ok(await _traspasoService.GetValidacionesAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<SolicitudTraspasoDto>> CrearSolicitud(SolicitudTraspasoCreateDto dto)
        {
            var created = await _traspasoService.CrearSolicitudAsync(dto);
            return CreatedAtAction(nameof(GetSolicitud), new { id = created.Id }, created);
        }

        [HttpPost("{id}/aceptar-origen")]
        public async Task<ActionResult<SolicitudTraspasoDto>> AceptarOrigen(int id)
        {
            return Ok(await _traspasoService.AceptarOrigenAsync(id));
        }

        [HttpPost("{id}/rechazar-origen")]
        public async Task<ActionResult<SolicitudTraspasoDto>> RechazarOrigen(int id, TraspasoMotivoDto dto)
        {
            return Ok(await _traspasoService.RechazarOrigenAsync(id, dto));
        }

        [HttpPost("{id}/aprobar")]
        public async Task<ActionResult<SolicitudTraspasoDto>> Aprobar(int id, [FromQuery] bool forzar = false)
        {
            return Ok(await _traspasoService.AprobarFederacionAsync(id, forzar));
        }

        [HttpPost("{id}/rechazar")]
        public async Task<ActionResult<SolicitudTraspasoDto>> Rechazar(int id, TraspasoMotivoDto dto)
        {
            return Ok(await _traspasoService.RechazarFederacionAsync(id, dto));
        }

        [HttpPost("{id}/cancelar")]
        public async Task<ActionResult<SolicitudTraspasoDto>> Cancelar(int id)
        {
            return Ok(await _traspasoService.CancelarAsync(id));
        }
    }
}
