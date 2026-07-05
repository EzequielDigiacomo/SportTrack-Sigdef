using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Entidades.DTOs.Club;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;
using SportTrack_Sigdef.Entidades.DTOs.EntrenadorFederacion;
using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;
using SportTrack_Sigdef.Entidades.DTOs.Evento;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public interface IClubServices
    {
        Task<ActionResult<ClubDetailDto>> GetClub(int id);
        Task<ActionResult<IEnumerable<ClubDto>>> GetClubes();
        Task<ActionResult<IEnumerable<AtletaDto>>> GetAtletasByClub(int id);
        Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadoresByClub(int id);
        Task<ActionResult<IEnumerable<DelegadoClubDto>>> GetDelegadosByClub(int id);
        Task<ActionResult<IEnumerable<EventoDto>>> GetEventosByClub(int id);
        Task<ActionResult<IEnumerable<ClubDto>>> SearchClubes(string term);
        Task<ActionResult<ClubDto>> PostClub(ClubCreateDto clubCreateDto);
        Task<IActionResult> PutClub(int id, ClubCreateDto clubCreateDto);
        Task<IActionResult> DeleteClub(int id);
    }
}
