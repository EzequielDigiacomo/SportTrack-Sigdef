using SportTrack_Sigdef.Controladores.Distancia.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Distancia
{
    public interface IDistanciaService
    {
        Task<IEnumerable<DistanciaDto>> GetAllDistanciasAsync();
        Task<DistanciaDto> GetDistanciaByIdAsync(int id);
        Task<DistanciaDto> CreateDistanciaAsync(DistanciaCreateDto distanciaDto);
        Task<DistanciaDto> UpdateDistanciaAsync(int id, DistanciaUpdateDto distanciaDto);
        Task<bool> DeleteDistanciaAsync(int id);
        Task<IEnumerable<DistanciaRegataDto>> GetDistanciasRegataAsync();
    }
}
