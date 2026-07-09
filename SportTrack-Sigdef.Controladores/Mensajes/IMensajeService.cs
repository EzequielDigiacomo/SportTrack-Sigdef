using SportTrack_Sigdef.Controladores.Mensajes.Dtos;
using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Mensajes
{
    public interface IMensajeService
    {
        Task<List<HiloListItemDto>> GetHilosAsync(string username);
        Task<HiloDetalleDto> GetHiloDetalleAsync(int hiloId, string username);
        Task<HiloDetalleDto> CrearHiloAsync(CrearHiloDto dto, string username);
        Task<HiloDetalleDto> ResponderHiloAsync(int hiloId, ResponderHiloDto dto, string username);
        Task MarcarHiloLeidoAsync(int hiloId, string username);
    }
}
