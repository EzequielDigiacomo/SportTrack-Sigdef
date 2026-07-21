using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Evento
{
    public interface IEventoEstadoSyncService
    {
        /// <summary>Sincroniza todos los eventos que aún pueden cambiar de estado.</summary>
        Task<int> SyncAllAsync(CancellationToken cancellationToken = default);

        /// <summary>Sincroniza un evento puntual si no está cancelado/finalizado.</summary>
        Task<bool> SyncEventoAsync(int eventoId, CancellationToken cancellationToken = default);
    }
}
