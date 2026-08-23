using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Resultado
{
    public interface IResultadoBatchUpdateService
    {
        Task<IEnumerable<Entidades.Entidades.Resultado>> ApplyBatchUpdateAsync(
            IEnumerable<ResultadoUpdateDto> dto,
            string? username);
    }
}
