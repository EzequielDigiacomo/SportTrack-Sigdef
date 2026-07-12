using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Controladores.Mensajes;
using SportTrack_Sigdef.Controladores.Mensajes.Dtos;
using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controllers
{
    [ApiController]
    [Route("api/mensajes")]
    [Authorize(Roles = "SuperAdmin,Admin,Club")]
    public class MensajesController : ControllerBase
    {
        private readonly IMensajeService _mensajeService;

        public MensajesController(IMensajeService mensajeService)
        {
            _mensajeService = mensajeService;
        }

        [HttpGet("hilos")]
        public async Task<IActionResult> GetHilos([FromQuery] int? campanaId = null)
        {
            var sistema = ResolveSistemaOrigen();
            var username = User.Identity?.Name;
            var result = await _mensajeService.GetHilosAsync(username!, sistema, campanaId);
            return Ok(result);
        }

        [HttpGet("hilos/{id:int}")]
        public async Task<IActionResult> GetHilo(int id)
        {
            var sistema = ResolveSistemaOrigen();
            var username = User.Identity?.Name;
            var result = await _mensajeService.GetHiloDetalleAsync(id, username!, sistema);
            return Ok(result);
        }

        [HttpPost("hilos")]
        public async Task<IActionResult> CrearHilo([FromBody] CrearHiloDto dto)
        {
            var sistema = ResolveSistemaOrigen();
            var username = User.Identity?.Name;
            var result = await _mensajeService.CrearHiloAsync(dto, username!, sistema);
            return CreatedAtAction(nameof(GetHilo), new { id = result.IdHilo }, result);
        }

        [HttpPost("hilos/masivo")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> EnviarMasivo([FromBody] EnviarMasivoDto dto)
        {
            var sistema = ResolveSistemaOrigen();
            var username = User.Identity?.Name;
            var result = await _mensajeService.EnviarMasivoAsync(dto, username!, sistema);
            return Ok(result);
        }

        [HttpPost("hilos/{id:int}/responder")]
        public async Task<IActionResult> ResponderHilo(int id, [FromBody] ResponderHiloDto dto)
        {
            var sistema = ResolveSistemaOrigen();
            var username = User.Identity?.Name;
            var result = await _mensajeService.ResponderHiloAsync(id, dto, username!, sistema);
            return Ok(result);
        }

        [HttpPatch("hilos/{id:int}/leer")]
        public async Task<IActionResult> MarcarLeido(int id)
        {
            var sistema = ResolveSistemaOrigen();
            var username = User.Identity?.Name;
            await _mensajeService.MarcarHiloLeidoAsync(id, username!, sistema);
            return Ok(new { message = "Hilo marcado como leído" });
        }

        [HttpGet("no-leidos/count")]
        public async Task<IActionResult> GetNoLeidosCount()
        {
            var sistema = ResolveSistemaOrigen();
            var username = User.Identity?.Name;
            var count = await _mensajeService.GetNoLeidosCountAsync(username!, sistema);
            return Ok(new { count });
        }

        [HttpGet("campanas")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetCampanas()
        {
            var sistema = ResolveSistemaOrigen();
            var username = User.Identity?.Name;
            var result = await _mensajeService.GetCampanasAsync(username!, sistema);
            return Ok(result);
        }

        [HttpGet("campanas/{id:int}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetCampana(int id)
        {
            var sistema = ResolveSistemaOrigen();
            var username = User.Identity?.Name;
            var result = await _mensajeService.GetCampanaDetalleAsync(id, username!, sistema);
            return Ok(result);
        }

        private string ResolveSistemaOrigen()
        {
            var clientApp = Request.Headers["X-Client-App"].FirstOrDefault();
            try
            {
                return MensajeriaSistemaOrigen.Normalizar(clientApp);
            }
            catch (ArgumentException ex)
            {
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
