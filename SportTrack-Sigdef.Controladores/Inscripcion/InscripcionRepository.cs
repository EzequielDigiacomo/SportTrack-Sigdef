using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.AccesoDatos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.Inscripcion;
using SportTrack_Sigdef.Controladores.Exceptions;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public class InscripcionRepository : IInscripcionRepository
    {
        private readonly SportTrackDbContext _context;

        public InscripcionRepository(SportTrackDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetAllAsync()
        {
            return await _context.Inscripciones
                .Include(i => i.Participante)
                    .ThenInclude(p => p.Club)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Evento)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Categoria)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Bote)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Distancia)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Sexo)
                .Include(i => i.Tripulantes)
                    .ThenInclude(t => t.Participante)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Entidades.Entidades.Inscripcion?> GetByIdAsync(int id)
        {
            return await _context.Inscripciones
                .Include(i => i.Participante)
                    .ThenInclude(p => p.Club)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Evento)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Categoria)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Bote)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Distancia)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Sexo)
                .Include(i => i.Tripulantes)
                    .ThenInclude(t => t.Participante)
                .FirstOrDefaultAsync(i => i.IdInscripcion == id);
        }

        public async Task<Entidades.Entidades.Inscripcion> CreateAsync(Entidades.Entidades.Inscripcion inscripcion)
        {
            var eventoPruebaExists = await _context.EventoPruebas
                .AnyAsync(ep => ep.IdEventoPrueba == inscripcion.IdEventoPrueba);
            if (!eventoPruebaExists)
                throw new NotFoundException($"La prueba del evento {inscripcion.IdEventoPrueba} no existe.");

            if (inscripcion.IdParticipante.HasValue)
            {
                var participanteExists = await _context.Participantes
                    .AnyAsync(p => p.ParticipanteId == inscripcion.IdParticipante);
                if (!participanteExists)
                    throw new NotFoundException($"El participante {inscripcion.IdParticipante} no existe.");

                var yaInscripto = await _context.Inscripciones.AnyAsync(i =>
                    i.IdEventoPrueba == inscripcion.IdEventoPrueba &&
                    i.IdParticipante == inscripcion.IdParticipante);
                if (yaInscripto)
                    throw new BadRequestException("El participante ya está inscripto en esta prueba.");

                await ValidarReglasInscripcionAsync(
                    inscripcion.IdEventoPrueba,
                    inscripcion.IdParticipante.Value);
            }

            _context.Inscripciones.Add(inscripcion);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new BadRequestException(
                    "No se pudo guardar la inscripción. Verifique los datos enviados.");
            }
            return inscripcion;
        }

        public async Task<Entidades.Entidades.Inscripcion> UpdateAsync(Entidades.Entidades.Inscripcion inscripcion)
        {
            _context.Entry(inscripcion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return inscripcion;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion == null)
                return false;

            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Inscripciones.AnyAsync(i => i.IdInscripcion == id);
        }

        public async Task<int> CountByEventoPruebaIdAsync(int eventoPruebaId)
        {
            return await _context.Inscripciones
                .CountAsync(i => i.IdEventoPrueba == eventoPruebaId);
        }

        public async Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetByEventoPruebaIdAsync(int eventoPruebaId)
        {
            return await _context.Inscripciones
                .Include(i => i.Participante)
                    .ThenInclude(p => p.Club)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Evento)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Categoria)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Bote)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Distancia)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Sexo)
                .Include(i => i.Tripulantes)
                    .ThenInclude(t => t.Participante)
                .Where(i => i.IdEventoPrueba == eventoPruebaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetByEventoAndClubAsync(int eventoId, int clubId)
        {
            return await _context.Inscripciones
                .Include(i => i.Participante)
                    .ThenInclude(p => p.Club)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Evento)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Categoria)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Bote)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Distancia)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Sexo)
                .Include(i => i.Tripulantes)
                    .ThenInclude(t => t.Participante)
                .Where(i => i.EventoPrueba.IdEvento == eventoId && 
                            (i.Participante != null && i.Participante.IdClub == clubId || i.Tripulantes.Any(t => t.Participante != null && t.Participante.IdClub == clubId)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetRegistroAsync(
            int? clubScope,
            int? federacionScope,
            int? eventoId,
            int? clubIdFilter,
            int? participanteId,
            string? busqueda)
        {
            var query = _context.Inscripciones
                .Include(i => i.Participante)
                    .ThenInclude(p => p!.Club)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Evento)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Categoria)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Bote)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Distancia)
                .Include(i => i.EventoPrueba)
                    .ThenInclude(ep => ep.Prueba)
                        .ThenInclude(p => p.Sexo)
                .Include(i => i.Tripulantes)
                    .ThenInclude(t => t.Participante)
                .AsNoTracking()
                .AsQueryable();

            if (clubScope.HasValue)
            {
                query = query.Where(i =>
                    (i.Participante != null && i.Participante.IdClub == clubScope.Value) ||
                    i.Tripulantes.Any(t => t.Participante != null && t.Participante.IdClub == clubScope.Value));
            }
            else if (federacionScope.HasValue)
            {
                query = query.Where(i =>
                    i.Participante != null &&
                    i.Participante.Club != null &&
                    i.Participante.Club.IdFederacion == federacionScope.Value);
            }

            if (eventoId.HasValue)
            {
                query = query.Where(i => i.EventoPrueba.IdEvento == eventoId.Value);
            }

            if (clubIdFilter.HasValue)
            {
                query = query.Where(i =>
                    i.Participante != null && i.Participante.IdClub == clubIdFilter.Value);
            }

            if (participanteId.HasValue)
            {
                query = query.Where(i => i.IdParticipante == participanteId.Value);
            }

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var term = busqueda.Trim().ToLower();
                query = query.Where(i =>
                    (i.Participante != null && (
                        i.Participante.Nombre.ToLower().Contains(term) ||
                        i.Participante.Apellido.ToLower().Contains(term) ||
                        (i.Participante.Documento != null && i.Participante.Documento.ToLower().Contains(term)))) ||
                    (i.EventoPrueba.Evento.Nombre.ToLower().Contains(term)) ||
                    (i.Participante != null && i.Participante.Club != null &&
                        i.Participante.Club.Nombre.ToLower().Contains(term)));
            }

            return await query
                .OrderByDescending(i => i.FechaInscripcion)
                .ThenBy(i => i.Participante != null ? i.Participante.Apellido : string.Empty)
                .ToListAsync();
        }

        public async Task ValidarReglasInscripcionAsync(int eventoPruebaId, int participanteId)
        {
            var eventoPrueba = await _context.EventoPruebas
                .Include(ep => ep.Evento)
                .Include(ep => ep.Prueba)
                .AsNoTracking()
                .FirstOrDefaultAsync(ep => ep.IdEventoPrueba == eventoPruebaId);

            if (eventoPrueba?.Evento == null || eventoPrueba.Prueba == null)
                throw new NotFoundException($"La prueba del evento {eventoPruebaId} no existe.");

            var participante = await _context.Participantes
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ParticipanteId == participanteId);

            if (participante == null)
                throw new NotFoundException($"El participante {participanteId} no existe.");

            InscripcionEligibilityHelper.ValidarSub23EnSenior(
                eventoPrueba.Evento,
                eventoPrueba.Prueba.CategoriaEdad,
                participante);
        }

        public async Task ReplaceTripulacionAsync(
            Entidades.Entidades.Inscripcion inscripcion,
            int nuevoTitularId,
            IEnumerable<(int ParticipanteId, int? PosicionEnBote)> tripulantes)
        {
            var existentes = await _context.InscripcionTripulantes
                .Where(t => t.InscripcionId == inscripcion.IdInscripcion)
                .ToListAsync();
            if (existentes.Count > 0)
                _context.InscripcionTripulantes.RemoveRange(existentes);

            inscripcion.IdParticipante = nuevoTitularId;
            _context.Entry(inscripcion).State = EntityState.Modified;

            foreach (var t in tripulantes)
            {
                _context.InscripcionTripulantes.Add(new InscripcionTripulante
                {
                    InscripcionId = inscripcion.IdInscripcion,
                    ParticipanteId = t.ParticipanteId,
                    PosicionEnBote = t.PosicionEnBote
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Entidades.Entidades.Participante?> GetParticipanteAsync(int participanteId)
        {
            return await _context.Participantes
                .Include(p => p.Club)
                .FirstOrDefaultAsync(p => p.ParticipanteId == participanteId);
        }

        public async Task SaveParticipanteAsync(Entidades.Entidades.Participante participante)
        {
            _context.Entry(participante).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveResultadosByInscripcionAsync(int inscripcionId, bool force)
        {
            var resultados = await _context.Resultados
                .Where(r => r.InscripcionId == inscripcionId)
                .ToListAsync();

            if (!force && resultados.Any(r => r.TiempoOficial.HasValue))
                throw new BadRequestException(
                    "Hay resultados con tiempo oficial. Usá force=true para forzar la baja.");

            if (resultados.Count > 0)
            {
                _context.Resultados.RemoveRange(resultados);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddResultadoAsync(Entidades.Entidades.Resultado resultado)
        {
            _context.Resultados.Add(resultado);
            await _context.SaveChangesAsync();
        }

        public async Task<Entidades.Entidades.Resultado?> GetResultadoByIdAsync(int resultadoId)
        {
            return await _context.Resultados
                .Include(r => r.Inscripcion)
                    .ThenInclude(i => i.Participante)
                .Include(r => r.Fase)
                .FirstOrDefaultAsync(r => r.Id == resultadoId);
        }

        public async Task SaveResultadosAsync(IEnumerable<Entidades.Entidades.Resultado> resultados)
        {
            foreach (var r in resultados)
                _context.Entry(r).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Entidades.Entidades.Fase?> GetFaseByIdAsync(int faseId)
        {
            return await _context.Fases
                .Include(f => f.Etapa)
                .Include(f => f.Resultados)
                .FirstOrDefaultAsync(f => f.Id == faseId);
        }

        public async Task<Entidades.Entidades.Fase?> GetPrimeraFaseByEventoPruebaAsync(int eventoPruebaId)
        {
            return await _context.Fases
                .Include(f => f.Etapa)
                .Include(f => f.Resultados)
                .Where(f => f.Etapa.EventoPruebaId == eventoPruebaId)
                .OrderBy(f => f.Etapa.Orden)
                .ThenBy(f => f.NumeroFase)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Entidades.Entidades.Resultado>> GetResultadosByFaseAsync(int faseId)
        {
            return await _context.Resultados
                .Where(r => r.FaseId == faseId)
                .ToListAsync();
        }

        public async Task<Entidades.Entidades.EventoPrueba?> GetEventoPruebaAsync(int eventoPruebaId)
        {
            return await _context.EventoPruebas
                .Include(ep => ep.Evento)
                .Include(ep => ep.Prueba)
                    .ThenInclude(p => p.Bote)
                .FirstOrDefaultAsync(ep => ep.IdEventoPrueba == eventoPruebaId);
        }
    }
}
