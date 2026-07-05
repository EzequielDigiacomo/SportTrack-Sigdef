using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Entidades.DTOs.Prueba;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public interface IPruebaServices
    {
        Task<ActionResult<PruebaDto>> GetPrueba(int id);
        Task<ActionResult<IEnumerable<PruebaDto>>> GetPruebas();
        Task<ActionResult<PruebaDto>> PostPrueba(PruebaCreateDto pruebaDto);
        Task<IActionResult> PutPrueba(int id, PruebaCreateDto pruebaDto);
        Task<IActionResult> DeletePrueba(int id);
    }
}
