using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Fase
{
    public interface IFaseRepository
    {
        Task<IEnumerable<Entidades.Entidades.Fase>> GetByEventoPruebaIdAsync(int eventoPruebaId);
        Task<Entidades.Entidades.Fase?> GetByIdAsync(int id);
        Task<Entidades.Entidades.Fase> CreateAsync(Entidades.Entidades.Fase fase);
        Task<IEnumerable<Entidades.Entidades.Fase>> CreateManyAsync(IEnumerable<Entidades.Entidades.Fase> fases);
        Task DeleteByEventoPruebaIdAsync(int eventoPruebaId);
        Task DeleteAsync(int id);
        Task<Entidades.Entidades.Fase> UpdateAsync(Entidades.Entidades.Fase fase);
        Task<IEnumerable<Entidades.Entidades.Fase>> GetByEventoIdAsync(int eventoId);
        Task<SportTrack_Sigdef.Entidades.Entidades.Resultado?> GetResultadoByIdAsync(int id);
        Task UpdateResultadoAsync(SportTrack_Sigdef.Entidades.Entidades.Resultado resultado);
        Task<int?> GetEventoIdByFaseIdAsync(int faseId);
        Task<int?> GetEventoIdByResultadoIdAsync(int resultadoId);
        Task<(int? EventoId, int? EventoPruebaId)> GetScopeByFaseIdAsync(int faseId);
    }

    public class FaseRepository : IFaseRepository
    {
        private readonly SportTrackDbContext _context;

        public FaseRepository(SportTrackDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entidades.Entidades.Fase>> GetByEventoPruebaIdAsync(int eventoPruebaId)
        {
            return await _context.Fases
                .AsNoTracking()
                .Include(f => f.Etapa)
                    .ThenInclude(e => e.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                            .ThenInclude(p => p.Categoria)
                .Include(f => f.Etapa)
                    .ThenInclude(e => e.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                            .ThenInclude(p => p.Bote)
                .Include(f => f.Etapa)
                    .ThenInclude(e => e.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                            .ThenInclude(p => p.Distancia)
                .Include(f => f.Resultados)
                    .ThenInclude(r => r.Inscripcion)
                        .ThenInclude(i => i.Participante)
                            .ThenInclude(p => p.Club)
                .Include(f => f.Resultados)
                    .ThenInclude(r => r.Inscripcion)
                        .ThenInclude(i => i.Tripulantes)
                            .ThenInclude(t => t.Participante)
                .Where(f => f.Etapa.EventoPruebaId == eventoPruebaId)
                .OrderBy(f => f.Etapa.Orden)
                .ThenBy(f => f.NumeroFase)
                .ToListAsync();
        }

        public async Task<Entidades.Entidades.Fase?> GetByIdAsync(int id)
        {
            return await _context.Fases
                .Include(f => f.Etapa)
                    .ThenInclude(e => e.EventoPrueba)
                        .ThenInclude(ep => ep.Inscripciones)
                .Include(f => f.Etapa)
                .Include(f => f.Resultados)
                    .ThenInclude(r => r.Inscripcion)
                        .ThenInclude(i => i.Participante)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Entidades.Entidades.Fase> CreateAsync(Entidades.Entidades.Fase fase)
        {
            _context.Fases.Add(fase);
            await _context.SaveChangesAsync();
            return fase;
        }

        public async Task<IEnumerable<Entidades.Entidades.Fase>> CreateManyAsync(IEnumerable<Entidades.Entidades.Fase> fases)
        {
            _context.Fases.AddRange(fases);
            await _context.SaveChangesAsync();
            return fases;
        }

        public async Task DeleteByEventoPruebaIdAsync(int eventoPruebaId)
        {
            var fases = await _context.Fases.Where(f => f.Etapa.EventoPruebaId == eventoPruebaId).ToListAsync();
            _context.Fases.RemoveRange(fases);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var f = await _context.Fases.FirstOrDefaultAsync(x => x.Id == id);
            if (f != null) {
                _context.Fases.Remove(f);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Entidades.Entidades.Fase> UpdateAsync(Entidades.Entidades.Fase fase)
        {
            _context.Fases.Update(fase);
            await _context.SaveChangesAsync();
            return fase;
        }

        public async Task<IEnumerable<Entidades.Entidades.Fase>> GetByEventoIdAsync(int eventoId)
        {
            // Lectura Live: sin Inscripciones de EventoPrueba (CantidadInscritos no es crítico aquí).
            return await _context.Fases
                .AsNoTracking()
                .Include(f => f.Etapa)
                    .ThenInclude(e => e.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                            .ThenInclude(p => p.Categoria)
                .Include(f => f.Etapa)
                    .ThenInclude(e => e.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                            .ThenInclude(p => p.Bote)
                .Include(f => f.Etapa)
                    .ThenInclude(e => e.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                            .ThenInclude(p => p.Distancia)
                .Include(f => f.Resultados)
                    .ThenInclude(r => r.Inscripcion)
                        .ThenInclude(i => i.Participante)
                            .ThenInclude(p => p.Club)
                .Include(f => f.Resultados)
                    .ThenInclude(r => r.Inscripcion)
                        .ThenInclude(i => i.Tripulantes)
                            .ThenInclude(t => t.Participante)
                .Where(f => f.Etapa.EventoPrueba.IdEvento == eventoId)
                .OrderBy(f => f.Etapa.EventoPrueba.FechaHora)
                .ThenBy(f => f.Etapa.Orden)
                .ThenBy(f => f.NumeroFase)
                .ToListAsync();
        }

        public async Task<SportTrack_Sigdef.Entidades.Entidades.Resultado?> GetResultadoByIdAsync(int id)
        {
            return await _context.Resultados.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateResultadoAsync(SportTrack_Sigdef.Entidades.Entidades.Resultado resultado)
        {
            _context.Resultados.Update(resultado);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetEventoIdByFaseIdAsync(int faseId)
        {
            return await _context.Fases
                .AsNoTracking()
                .Where(f => f.Id == faseId)
                .Select(f => (int?)f.Etapa.EventoPrueba.IdEvento)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetEventoIdByResultadoIdAsync(int resultadoId)
        {
            return await _context.Resultados
                .AsNoTracking()
                .Where(r => r.Id == resultadoId)
                .Select(r => (int?)r.Fase.Etapa.EventoPrueba.IdEvento)
                .FirstOrDefaultAsync();
        }

        public async Task<(int? EventoId, int? EventoPruebaId)> GetScopeByFaseIdAsync(int faseId)
        {
            var row = await _context.Fases
                .AsNoTracking()
                .Where(f => f.Id == faseId)
                .Select(f => new
                {
                    EventoId = (int?)f.Etapa.EventoPrueba.IdEvento,
                    EventoPruebaId = (int?)f.Etapa.EventoPruebaId
                })
                .FirstOrDefaultAsync();

            return row is null ? (null, null) : (row.EventoId, row.EventoPruebaId);
        }
    }
}
