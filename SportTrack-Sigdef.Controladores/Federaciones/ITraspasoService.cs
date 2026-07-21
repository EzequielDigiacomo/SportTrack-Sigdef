using SportTrack_Sigdef.Entidades.DTOs.Traspaso;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public interface ITraspasoService
    {
        Task<IEnumerable<PeriodoTraspasoDto>> GetPeriodosAsync();
        Task<PeriodoTraspasoDto?> GetPeriodoActivoAsync();
        Task<PeriodoTraspasoDto> CreatePeriodoAsync(PeriodoTraspasoCreateDto dto);
        Task<PeriodoTraspasoDto> UpdatePeriodoAsync(int id, PeriodoTraspasoUpdateDto dto);

        Task<IEnumerable<SolicitudTraspasoDto>> GetSolicitudesAsync(string? estado = null);
        Task<SolicitudTraspasoDto> GetSolicitudByIdAsync(int id);
        Task<TraspasoValidacionDto> GetValidacionesAsync(int id);

        Task<SolicitudTraspasoDto> CrearSolicitudAsync(SolicitudTraspasoCreateDto dto);
        Task<SolicitudTraspasoDto> AceptarOrigenAsync(int id);
        Task<SolicitudTraspasoDto> RechazarOrigenAsync(int id, TraspasoMotivoDto dto);
        Task<SolicitudTraspasoDto> AprobarFederacionAsync(int id, bool forzar = false);
        Task<SolicitudTraspasoDto> RechazarFederacionAsync(int id, TraspasoMotivoDto dto);
        Task<SolicitudTraspasoDto> CancelarAsync(int id);
        Task<IEnumerable<AtletaTraspasoBusquedaDto>> BuscarAtletasAsync(string term);
        Task<IEnumerable<TraspasoAuditoriaDto>> GetAuditoriaAsync(int limit = 50);
        Task<byte[]> ExportSolicitudesCsvAsync(int? periodoId = null, string? estado = null);
    }
}
