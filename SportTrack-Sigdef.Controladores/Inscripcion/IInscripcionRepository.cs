using SportTrack_Sigdef.Entidades.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public interface IInscripcionRepository
    {
        Task<Entidades.Entidades.Inscripcion?> GetByIdAsync(int id);
        Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetAllAsync();
        Task<Entidades.Entidades.Inscripcion> CreateAsync(Entidades.Entidades.Inscripcion inscripcion);
        Task<Entidades.Entidades.Inscripcion> UpdateAsync(Entidades.Entidades.Inscripcion inscripcion);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // Método solicitado para contabilizar inscripciones
        Task<int> CountByEventoPruebaIdAsync(int eventoPruebaId);
        
        Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetByEventoPruebaIdAsync(int eventoPruebaId);
        Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetByEventoAndClubAsync(int eventoId, int clubId);

        Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetRegistroAsync(
            int? clubScope,
            int? federacionScope,
            int? eventoId,
            int? clubIdFilter,
            int? participanteId,
            string? busqueda);

        Task ValidarReglasInscripcionAsync(int eventoPruebaId, int participanteId);

        Task ReplaceTripulacionAsync(
            Entidades.Entidades.Inscripcion inscripcion,
            int nuevoTitularId,
            IEnumerable<(int ParticipanteId, int? PosicionEnBote)> tripulantes);

        Task<Entidades.Entidades.Participante?> GetParticipanteAsync(int participanteId);
        Task SaveParticipanteAsync(Entidades.Entidades.Participante participante);
        Task RemoveResultadosByInscripcionAsync(int inscripcionId, bool force);
        Task AddResultadoAsync(Entidades.Entidades.Resultado resultado);
        Task<Entidades.Entidades.Resultado?> GetResultadoByIdAsync(int resultadoId);
        Task SaveResultadosAsync(IEnumerable<Entidades.Entidades.Resultado> resultados);
        Task<Entidades.Entidades.Fase?> GetFaseByIdAsync(int faseId);
        Task<Entidades.Entidades.Fase?> GetPrimeraFaseByEventoPruebaAsync(int eventoPruebaId);
        Task<IEnumerable<Entidades.Entidades.Resultado>> GetResultadosByFaseAsync(int faseId);
        Task<Entidades.Entidades.EventoPrueba?> GetEventoPruebaAsync(int eventoPruebaId);
    }
}
