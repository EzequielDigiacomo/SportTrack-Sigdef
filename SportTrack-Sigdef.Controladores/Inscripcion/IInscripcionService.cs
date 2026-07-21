using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public interface IInscripcionService
    {
        Task<IEnumerable<InscripcionDto>> GetAllInscripcionesAsync();
        Task<InscripcionDto> GetInscripcionByIdAsync(int id);
        Task<InscripcionDto> CreateInscripcionAsync(InscripcionCreateDto inscripcionDto);
        Task<InscripcionDto> UpdateInscripcionAsync(int id, InscripcionUpdateDto inscripcionDto);
        Task<bool> DeleteInscripcionAsync(int id, bool allowWhenClosed = false);
        Task<int> GetCountByEventoPruebaIdAsync(int eventoPruebaId);
        Task<IEnumerable<InscripcionDto>> GetInscripcionesByEventoPruebaIdAsync(int eventoPruebaId);
        Task<IEnumerable<InscripcionDto>> GetInscripcionesByEventoAndClubAsync(int eventoId, int clubId);
        Task<bool> ToggleEsCabezaDeSerieAsync(int id);

        Task<IEnumerable<RegistroInscripcionDto>> GetRegistroInscripcionesAsync(
            int? clubScope,
            int? federacionScope,
            int? eventoId,
            int? clubIdFilter,
            int? participanteId,
            string? busqueda);
    }
}
