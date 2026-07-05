using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Entidades.DTOs.Federacion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public interface IFederacionServices
    {
        Task<ActionResult<FederacionDto>> GetFederacion(int id);
        Task<ActionResult<IEnumerable<FederacionDto>>> GetFederaciones();
        Task<ActionResult<FederacionDto>> PostFederacion(FederacionCreateDto federacionCreateDto);
        Task<IActionResult> PutFederacion(int id, FederacionCreateDto federacionCreateDto);
        Task<IActionResult> DeleteFederacion(int id);
    }
}
