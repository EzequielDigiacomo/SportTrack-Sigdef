using SIGDEF.DTOs;
using SportTrack_Sigdef.Controladores.Participante.Dtos;
using SportTrack_Sigdef.Entidades.DTOs.Participante;
using SportTrack_Sigdef.Entidades.Entidades;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public interface IAltaAtletaService
    {
        string NormalizarDocumento(string? documento);

        Task<Entidades.Entidades.Participante?> BuscarPorDocumentoAsync(string documento);

        Task<AltaAtletaResult> UpsertParticipanteAsync(AltaAtletaParticipanteInput input);

        Task<AtletaFederacion> EnsureAtletaFederacionAsync(int participanteId, AltaAtletaFederacionInput input);

        Task<AltaAtletaResult> AltaAtletaCompletaAsync(
            AltaAtletaParticipanteInput participanteInput,
            AltaAtletaFederacionInput? federacionInput = null);

        AltaAtletaParticipanteInput FromPersonaCreateDto(PersonaCreateDto dto, int? idClub = null);

        AltaAtletaParticipanteInput FromParticipanteCreateDto(ParticipanteCreateDto dto);

        AltaAtletaFederacionInput FromAtletaCreateDto(AtletaCreateDto dto);

        AltaAtletaFederacionInput DefaultsFromClub(int? idClub, int? idFederacion = null);
    }
}
