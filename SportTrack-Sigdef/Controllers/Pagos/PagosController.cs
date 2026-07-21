using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Pago;
using SportTrack_Sigdef.Controladores.Pago.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers.Pagos
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PagosController : ControllerBase
    {
        private readonly IPagoService _pagoService;

        public PagosController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        [HttpGet("historial")]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetHistorial([FromQuery] int? idFederacion = null)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var federacionClaim = User.FindFirst("FederacionId")?.Value;
            var clubClaim = User.FindFirst("ClubId")?.Value;

            int? scopeId = null;

            // SuperAdmin puede acotar por federación vía query.
            if ((role == "SuperAdmin" || role == "soporte_tecnico") && idFederacion.HasValue && idFederacion.Value > 0)
            {
                scopeId = idFederacion.Value;
                // Reutilizar filtro de Admin por federación.
                role = "Admin";
            }
            else if (role == "Admin")
            {
                if (int.TryParse(federacionClaim, out int fedId) && fedId > 0)
                    scopeId = fedId;
            }
            else if (int.TryParse(clubClaim, out int clubId) && clubId > 0)
            {
                scopeId = clubId;
            }

            var result = await _pagoService.GetHistorialPagosAsync(scopeId, role);
            return Ok(result);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<PagoDto>> RegistrarPago(RegistrarPagoDto dto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst(ClaimTypes.Name)?.Value 
                           ?? "System";

            var result = await _pagoService.RegistrarPagoAsync(dto, username);
            return Ok(result);
        }

        [HttpPut("clubes/{id}/toggle")]
        public async Task<IActionResult> ToggleClubPago(int id, [FromBody] bool alDia)
        {
            var result = await _pagoService.ToggleClubPagoStatusAsync(id, alDia);
            if (!result) return BadRequest("No se pudo actualizar el estado de pago del club");
            return NoContent();
        }

        [HttpPut("clubes/{id}/solicitar-pago")]
        public async Task<IActionResult> SolicitarPago(int id, [FromBody] bool pendiente)
        {
            var result = await _pagoService.SetSolicitudPagoPendienteAsync(id, pendiente);
            if (!result) return BadRequest("No se pudo registrar la solicitud de pago");
            return NoContent();
        }

        [HttpPut("atletas/{id}/toggle")]
        public async Task<IActionResult> ToggleAtletaPago(int id, [FromBody] bool alDia)
        {
            var result = await _pagoService.ToggleAtletaPagoStatusAsync(id, alDia);
            if (!result) return BadRequest("No se pudo actualizar el estado de pago del atleta");
            return NoContent();
        }

        [HttpPut("inscripciones/{id}/toggle")]
        public async Task<IActionResult> ToggleInscripcionPago(int id, [FromBody] bool pagado)
        {
            var result = await _pagoService.ToggleInscripcionPagoStatusAsync(id, pagado);
            if (!result) return BadRequest("No se pudo actualizar el estado de pago de la inscripción");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarPago(int id)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst(ClaimTypes.Name)?.Value
                           ?? "System";

            var result = await _pagoService.EliminarPagoAsync(id, username);
            if (!result) return BadRequest("No se pudo eliminar el recibo");
            return NoContent();
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> EliminarPagos([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest("Debés seleccionar al menos un recibo.");

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst(ClaimTypes.Name)?.Value
                           ?? "System";

            var deleted = await _pagoService.EliminarPagosAsync(ids, username);
            return Ok(new { eliminados = deleted });
        }
    }
}

