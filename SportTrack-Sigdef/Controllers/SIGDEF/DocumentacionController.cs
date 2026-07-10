using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Documentacion;
using SportTrack_Sigdef.Entidades.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGDEF.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentacionController : ControllerBase
    {
        private readonly IDocumentacionService _documentacionService;

        public DocumentacionController(IDocumentacionService documentacionService)
        {
            _documentacionService = documentacionService;
        }

        /// <summary>POST /api/Documentacion/upload</summary>
        [HttpPost("upload")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] DocumentoUploadDto dto)
        {
            try
            {
                if (dto.File == null || dto.File.Length == 0)
                    return BadRequest(new { success = false, error = "El archivo es obligatorio." });

                if (dto.PersonaId <= 0)
                    return BadRequest(new { success = false, error = "PersonaId inválido." });

                var result = await _documentacionService.UploadAsync(dto.File, dto.PersonaId, dto.TipoDocumento);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>GET /api/Documentacion/persona/{id}</summary>
        [HttpGet("persona/{id:int}")]
        public async Task<ActionResult<IEnumerable<object>>> GetByPersona(int id)
        {
            var docs = await _documentacionService.GetByPersonaAsync(id);
            return Ok(docs);
        }

        /// <summary>DELETE /api/Documentacion/{id}</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _documentacionService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { success = false, error = $"No se encontró el documento con ID {id}" });

            return Ok(new { success = true, message = "Documento eliminado correctamente" });
        }
    }
}
