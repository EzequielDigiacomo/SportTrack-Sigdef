using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public interface ITokenAccesoCambiosService
    {
        Task<IEnumerable<TokenAccesoCambiosDto>> GenerarAsync(int eventoId, TokenAccesoCambiosCreateDto dto, string? creadoPor);
        Task<IEnumerable<TokenAccesoCambiosDto>> ListarPorEventoAsync(int eventoId);
        Task RevocarAsync(int eventoId, int tokenId);
        Task<CambiosAccesoResumenDto> GetResumenByTokenAsync(string token);
        Task<InscripcionDto> UpdateTripulacionByTokenAsync(string token, int inscripcionId, InscripcionTripulacionUpdateDto dto);
    }
}
