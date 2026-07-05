using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Entidades.DTOs.Participante;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public interface IPersonaServices
    {
        Task<ActionResult<PersonaDetailDto>> GetPersona(int id);
        Task<ActionResult<IEnumerable<PersonaDto>>> GetPersonas();
        Task<ActionResult<PersonaDto>> GetPersonaByDocumento(string documento);
        Task<ActionResult<PersonaDto>> PostPersona(PersonaCreateDto personaCreateDto);
        Task<IActionResult> PutPersona(int id, PersonaCreateDto personaCreateDto);
        Task<IActionResult> DeletePersona(int id);
    }
}
