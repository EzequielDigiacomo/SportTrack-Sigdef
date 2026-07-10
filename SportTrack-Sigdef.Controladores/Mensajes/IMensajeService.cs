using SportTrack_Sigdef.Controladores.Mensajes.Dtos;

namespace SportTrack_Sigdef.Controladores.Mensajes
{
    public interface IMensajeService
    {
        Task<List<HiloListItemDto>> GetHilosAsync(string username, int? campanaId = null);
        Task<HiloDetalleDto> GetHiloDetalleAsync(int hiloId, string username);
        Task<HiloDetalleDto> CrearHiloAsync(CrearHiloDto dto, string username);
        Task<EnviarMasivoResultDto> EnviarMasivoAsync(EnviarMasivoDto dto, string username);
        Task<List<CampanaListItemDto>> GetCampanasAsync(string username);
        Task<CampanaDetalleDto> GetCampanaDetalleAsync(int campanaId, string username);
        Task<HiloDetalleDto> ResponderHiloAsync(int hiloId, ResponderHiloDto dto, string username);
        Task MarcarHiloLeidoAsync(int hiloId, string username);
    }
}
