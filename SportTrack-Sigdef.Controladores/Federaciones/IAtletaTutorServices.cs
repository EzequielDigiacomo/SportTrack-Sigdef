using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public interface IAtletaTutorServices
    {
        Task<ActionResult<AtletaTutorDetailDto>> GetAtletaTutor(int ParticipanteId, int idTutor);
        Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetAtletasTutores();
        Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetAtletasPorTutor(int idTutor);
        Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetTutoresPorAtleta(int ParticipanteId);
        Task<ActionResult<AtletaTutorDto>> PostAtletaTutor(AtletaTutorCreateDto atletaTutorCreateDto);
        Task<IActionResult> PutAtletaTutor(int ParticipanteId, int idTutor, AtletaTutorCreateDto atletaTutorCreateDto);
        Task<IActionResult> DeleteAtletaTutor(int ParticipanteId, int idTutor);
    }
}
