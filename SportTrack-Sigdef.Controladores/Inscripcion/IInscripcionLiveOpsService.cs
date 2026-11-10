using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public interface IInscripcionLiveOpsService
    {
        Task<InscripcionDto> UpdateTripulacionAdminAsync(int inscripcionId, InscripcionTripulacionUpdateDto dto);
        Task PatchParticipanteNombreAsync(int participanteId, ParticipanteNombrePatchDto dto);
        Task<InscripcionDto> UpdateNumeroCompetidorAsync(int inscripcionId, string numeroCompetidor);
        Task ReasignarCarrilesAsync(int faseId, LiveCarrilesBatchDto dto);
        Task BajaBoteAsync(int inscripcionId, bool force);
        Task<InscripcionDto> AgregarBoteAsync(LiveAgregarBoteDto dto);
        Task<InscripcionDto> MoverPruebaAsync(int inscripcionId, LiveMoverPruebaDto dto);
    }
}
