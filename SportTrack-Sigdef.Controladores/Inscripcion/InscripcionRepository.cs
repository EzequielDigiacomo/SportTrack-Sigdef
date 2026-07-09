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
    }
}
