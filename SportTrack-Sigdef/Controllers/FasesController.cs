using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Fase;
using SportTrack_Sigdef.Controladores.Fase.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FasesController : ControllerBase
    {
        private readonly IFaseService _faseService;

        public FasesController(IFaseService faseService)
        {
            _faseService = faseService;
        }

        // Lectura anónima: Live + paneles (GET)
        [HttpGet("EventoPrueba/{eventoPruebaId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FaseDto>>> GetFasesPorEventoPrueba(int eventoPruebaId)
        {
            var fases = await _faseService.GetFasesPorEventoPruebaAsync(eventoPruebaId);
            return Ok(fases);
        }

        [HttpGet("all-by-evento/{eventoId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FaseDto>>> GetFasesPorEvento(int eventoId)
        {
            var fases = await _faseService.GetFasesPorEventoAsync(eventoId);
            return Ok(fases);
        }

        [HttpGet("ProgresionAudit/{eventoPruebaId}")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult<IEnumerable<ProgressionAuditDto>>> GetProgresionAudit(int eventoPruebaId)
        {
            var audit = await _faseService.GetProgressionAuditAsync(eventoPruebaId);
            return Ok(audit);
        }

        [HttpPost("BatchUpdate")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult> BatchUpdate([FromBody] List<FaseBatchUpdateDto> dto)
        {
            await _faseService.BatchUpdateFasesAsync(dto);
            return Ok();
        }

        [HttpPost("Generar/{eventoPruebaId}")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult<IEnumerable<FaseDto>>> GenerarFases(int eventoPruebaId)
        {
            var fases = await _faseService.GenerarFasesAutoAsync(eventoPruebaId);
            return Ok(fases);
        }

        [HttpPost("GenerarManual/{eventoPruebaId}")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult<IEnumerable<FaseDto>>> GenerarFasesManual(int eventoPruebaId, [FromBody] List<ManualPlacementDto> placements)
        {
            var fases = await _faseService.GenerarFasesManualAsync(eventoPruebaId, placements);
            return Ok(fases);
        }

        [HttpPost("Promover/{eventoPruebaId}")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult<IEnumerable<FaseDto>>> Promover(int eventoPruebaId)
        {
            var fases = await _faseService.PromoverFasesAsync(eventoPruebaId);
            return Ok(fases);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult> Delete(int id)
        {
            await _faseService.DeleteFaseAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/Iniciar")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult<FaseDto>> Iniciar(int id, [FromQuery] DateTime? startTime = null)
        {
            var fase = await _faseService.IniciarFaseAsync(id, startTime);
            return Ok(fase);
        }

        [HttpPost("{id}/Finalizar")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult<FaseDto>> Finalizar(int id)
        {
            var fase = await _faseService.FinalizarFaseAsync(id);
            return Ok(fase);
        }

        [HttpPost("{id}/Reiniciar")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult<FaseDto>> Reiniciar(int id)
        {
            var fase = await _faseService.ReiniciarFaseAsync(id);
            return Ok(fase);
        }

        [HttpPost("{id}/EnviarARevision")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult<FaseDto>> EnviarARevision(int id)
        {
            var fase = await _faseService.EnviarARevisionAsync(id);
            return Ok(fase);
        }
    }
}
