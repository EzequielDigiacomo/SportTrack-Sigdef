using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;
using SportTrack_Sigdef.Controladores.Federaciones;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGDEF.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtletaTutorController : ControllerBase
    {
        private readonly IAtletaTutorServices _atletaTutorServices;

        public AtletaTutorController(IAtletaTutorServices atletaTutorServices)
        {
            _atletaTutorServices = atletaTutorServices;
        }

        // GET: api/AtletaTutor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetAtletaTutores()
        {
            return await _atletaTutorServices.GetAtletasTutores();
        }

        // GET: api/AtletaTutor/{participanteId}/{idTutor}
        [HttpGet("{participanteId}/{idTutor}")]
        public async Task<ActionResult<AtletaTutorDetailDto>> GetAtletaTutor(int participanteId, int idTutor)
        {
            return await _atletaTutorServices.GetAtletaTutor(participanteId, idTutor);
        }

        // GET: api/AtletaTutor/atleta/{participanteId}
        [HttpGet("atleta/{participanteId}")]
        public async Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetTutoresPorAtleta(int participanteId)
        {
            return await _atletaTutorServices.GetTutoresPorAtleta(participanteId);
        }

        // GET: api/AtletaTutor/tutor/{idTutor}
        [HttpGet("tutor/{idTutor}")]
        public async Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetAtletasPorTutor(int idTutor)
        {
            return await _atletaTutorServices.GetAtletasPorTutor(idTutor);
        }

        // POST: api/AtletaTutor
        [HttpPost]
        public async Task<ActionResult<AtletaTutorDto>> PostAtletaTutor(AtletaTutorCreateDto atletaTutorCreateDto)
        {
            return await _atletaTutorServices.PostAtletaTutor(atletaTutorCreateDto);
        }

        // DELETE: api/AtletaTutor/{participanteId}/{idTutor}
        [HttpDelete("{participanteId}/{idTutor}")]
        public async Task<IActionResult> DeleteAtletaTutor(int participanteId, int idTutor)
        {
            return await _atletaTutorServices.DeleteAtletaTutor(participanteId, idTutor);
        }
    }
}

