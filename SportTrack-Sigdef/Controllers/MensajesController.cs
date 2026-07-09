using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Mensajes;
using SportTrack_Sigdef.Controladores.Mensajes.Dtos;

namespace SportTrack_Sigdef.Controllers
{
    [ApiController]
    [Route("api/mensajes")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class MensajesController : ControllerBase
    {
        private readonly IMensajeService _mensajeService;

        public MensajesController(IMensajeService mensajeService)
        {
            _mensajeService = mensajeService;
        }

        [HttpGet("hilos")]
        public async Task<IActionResult> GetHilos()
        {
            var username = User.Identity?.Name;
            var result = await _mensajeService.GetHilosAsync(username!);
            return Ok(result);
        }

        [HttpGet("hilos/{id:int}")]
        public async Task<IActionResult> GetHilo(int id)
        {
            var username = User.Identity?.Name;
            var result = await _mensajeService.GetHiloDetalleAsync(id, username!);
            return Ok(result);
        }

        [HttpPost("hilos")]
        public async Task<IActionResult> CrearHilo([FromBody] CrearHiloDto dto)
        {
            var username = User.Identity?.Name;
            var result = await _mensajeService.CrearHiloAsync(dto, username!);
            return CreatedAtAction(nameof(GetHilo), new { id = result.IdHilo }, result);
        }

        [HttpPost("hilos/{id:int}/responder")]
        public async Task<IActionResult> ResponderHilo(int id, [FromBody] ResponderHiloDto dto)
        {
            var username = User.Identity?.Name;
            var result = await _mensajeService.ResponderHiloAsync(id, dto, username!);
            return Ok(result);
        }

        [HttpPatch("hilos/{id:int}/leer")]
        public async Task<IActionResult> MarcarLeido(int id)
        {
            var username = User.Identity?.Name;
            await _mensajeService.MarcarHiloLeidoAsync(id, username!);
            return Ok(new { message = "Hilo marcado como leído" });
        }
    }
}
