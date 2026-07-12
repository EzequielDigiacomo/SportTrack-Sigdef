using SportTrack_Sigdef.Controladores.Mensajes.Dtos;

namespace SportTrack_Sigdef.Controladores.Mensajes
{
    public interface IMensajeService
    {
        Task<List<HiloListItemDto>> GetHilosAsync(string username, string sistemaOrigen, int? campanaId = null);
        Task<HiloDetalleDto> GetHiloDetalleAsync(int hiloId, string username, string sistemaOrigen);
        Task<HiloDetalleDto> CrearHiloAsync(CrearHiloDto dto, string username, string sistemaOrigen);
        Task<EnviarMasivoResultDto> EnviarMasivoAsync(EnviarMasivoDto dto, string username, string sistemaOrigen);
        Task<List<CampanaListItemDto>> GetCampanasAsync(string username, string sistemaOrigen);
        Task<CampanaDetalleDto> GetCampanaDetalleAsync(int campanaId, string username, string sistemaOrigen);
        Task<HiloDetalleDto> ResponderHiloAsync(int hiloId, ResponderHiloDto dto, string username, string sistemaOrigen);
        Task MarcarHiloLeidoAsync(int hiloId, string username, string sistemaOrigen);
        Task<int> GetNoLeidosCountAsync(string username, string sistemaOrigen);
    }
}
