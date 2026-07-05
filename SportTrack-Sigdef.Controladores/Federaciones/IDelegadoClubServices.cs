using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public interface IDelegadoClubServices
    {
        Task<ActionResult<DelegadoClubDetailDto>> GetDelegadoClub(int id);
        Task<ActionResult<IEnumerable<DelegadoClubDto>>> GetDelegadosClub();
        Task<ActionResult<IEnumerable<DelegadoClubDto>>> GetDelegadosPorFederacion(int idFederacion);
        Task<ActionResult<DelegadoClubDto>> PostDelegadoClub(DelegadoClubCreateDto delegadoClubCreateDto);
        Task<IActionResult> PutDelegadoClub(int id, DelegadoClubCreateDto delegadoClubCreateDto);
        Task<IActionResult> DeleteDelegadoClub(int id);
    }
}
