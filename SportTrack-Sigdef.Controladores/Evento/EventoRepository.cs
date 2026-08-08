using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Evento
{
    public class EventoRepository : IEventoRepository
    {
        private readonly SportTrackDbContext _context;

        private static readonly string[] RolesAdministrativos =
            { "Admin", "Largador", "Cronometrista", "JuezControl", "Control" };

        public EventoRepository(SportTrackDbContext context)
        {
            _context = context;
        }

        private static bool IsSuperAdmin(string? rol) =>
            string.Equals(rol?.Trim(), "SuperAdmin", StringComparison.OrdinalIgnoreCase);

        private static bool IsRolAdministrativo(string? rol) =>
            RolesAdministrativos.Any(r => r.Equals(rol?.Trim(), StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Eventos de una federación: IdFederacion directo o IdClub de un club afiliado.
        /// Nunca interpreta federationId como IdClub (evita colisión Club 1 / Federación 1).
        /// </summary>
        private async Task<IQueryable<Entidades.Entidades.Evento>> ApplyFederationScopeAsync(
            IQueryable<Entidades.Entidades.Evento> query,
            int federationId)
        {
            var clubIds = await _context.Clubes
                .Where(c => c.IdFederacion == federationId)
                .Select(c => c.IdClub)
                .ToListAsync();

            return query.Where(e =>
                e.IdFederacion == federationId ||
                (e.IdClub.HasValue && clubIds.Contains(e.IdClub.Value)));
        }

        /// <summary>
        /// Scope por club: roles administrativos ven toda la federación del club;
        /// el resto ve eventos del club o de su federación.
        /// </summary>
        private async Task<IQueryable<Entidades.Entidades.Evento>> ApplyClubScopeAsync(
            IQueryable<Entidades.Entidades.Evento> query,
            int clubId,
            string? rol)
        {
            var clubActual = await _context.Clubes.AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdClub == clubId);

            if (clubActual?.IdFederacion is int federationId && federationId > 0)
            {
                if (IsRolAdministrativo(rol))
                    return await ApplyFederationScopeAsync(query, federationId);

                return query.Where(e =>
                    e.IdClub == clubId || e.IdFederacion == federationId);
            }

            return query.Where(e => e.IdClub == clubId);
        }

        private async Task<IQueryable<Entidades.Entidades.Evento>> ApplyScopeFilterAsync(
            IQueryable<Entidades.Entidades.Evento> query,
            int? clubId,
            int? federacionId,
            string? rol)
        {
            if (federacionId.HasValue && federacionId.Value > 0)
                return await ApplyFederationScopeAsync(query, federacionId.Value);

            if (clubId.HasValue && clubId.Value > 0)
                return await ApplyClubScopeAsync(query, clubId.Value, rol);

            // Sin scope: no filtrar (SuperAdmin) o lista vacía (otros)
            if (IsSuperAdmin(rol))
                return query;

            return query.Where(e => false);
        }

        public async Task<IEnumerable<Entidades.Entidades.Evento>> GetAllAsync(
            int? clubId = null,
            string? rol = null,
            int? federacionId = null)
        {
            var query = _context.Eventos.AsQueryable();

            // SuperAdmin sin filtro: catálogo global. Resto: siempre scoped (vacío si no hay tenant).
            if (!(IsSuperAdmin(rol) && !clubId.HasValue && !federacionId.HasValue))
            {
                query = await ApplyScopeFilterAsync(query, clubId, federacionId, rol);
            }

            return await query
                .Include(e => e.Club)
                .AsNoTracking()
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }

        public async Task<Entidades.Entidades.Evento?> GetByIdAsync(int id)
        {
            return await _context.Eventos
                .Include(e => e.Federacion!)
                    .ThenInclude(f => f.PlanSaaS)
                .Include(e => e.Club)
                .Include(e => e.EventoPruebas)
                    .ThenInclude(ep => ep.Prueba)
                .FirstOrDefaultAsync(e => e.IdEvento == id);
        }

        public async Task<Entidades.Entidades.Evento> CreateAsync(Entidades.Entidades.Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<Entidades.Entidades.Evento> UpdateAsync(Entidades.Entidades.Evento evento)
        {
            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return evento;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return false;
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Eventos.AnyAsync(e => e.IdEvento == id);
        }

        public async Task<IEnumerable<Entidades.Entidades.Evento>> GetProximosAsync(
            int? clubId = null,
            string? rol = null,
            int? federacionId = null)
        {
            var query = _context.Eventos
                .Where(e => e.Fecha >= DateTime.UtcNow.Date);

            // Endpoint público: sin scope no se filtra. Con club/federación, sí.
            if (clubId.HasValue || federacionId.HasValue)
            {
                query = await ApplyScopeFilterAsync(query, clubId, federacionId, rol);
            }

            return await query
                .OrderBy(e => e.Fecha)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<EventoPrueba>> GetPruebasByEventoIdAsync(int eventoId)
        {
            return await _context.EventoPruebas
                .AsNoTracking()
                .Include(ep => ep.Prueba)
                    .ThenInclude(p => p.Categoria)
                .Include(ep => ep.Prueba)
                    .ThenInclude(p => p.Bote)
                .Include(ep => ep.Prueba)
                    .ThenInclude(p => p.Distancia)
                .Include(ep => ep.Prueba)
                    .ThenInclude(p => p.Sexo)
                .Include(ep => ep.Inscripciones)
                .Where(ep => ep.IdEvento == eventoId)
                .OrderBy(ep => ep.FechaHora)
                .ToListAsync();
        }

        public async Task<EventoPrueba?> GetEventoPruebaByIdAsync(int id)
        {
            return await _context.EventoPruebas
                .Include(ep => ep.Evento)
                .FirstOrDefaultAsync(ep => ep.IdEventoPrueba == id);
        }

        public async Task<EventoPrueba> AssignPruebaAsync(EventoPrueba eventoPrueba)
        {
            _context.EventoPruebas.Add(eventoPrueba);
            await _context.SaveChangesAsync();
            return eventoPrueba;
        }

        public async Task<EventoPrueba> UpdateEventoPruebaAsync(EventoPrueba eventoPrueba)
        {
            _context.Entry(eventoPrueba).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return eventoPrueba;
        }

        public async Task<bool> UnassignPruebaAsync(int id)
        {
            var entity = await _context.EventoPruebas.FindAsync(id);
            if (entity == null) return false;
            
            _context.EventoPruebas.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> UnassignByGrupoLargadaAsync(Guid grupoLargadaId)
        {
            var entities = await _context.EventoPruebas
                .Where(ep => ep.GrupoLargadaId == grupoLargadaId)
                .ToListAsync();
            if (entities.Count == 0) return 0;

            _context.EventoPruebas.RemoveRange(entities);
            await _context.SaveChangesAsync();
            return entities.Count;
        }

        public async Task<IEnumerable<EventoPrueba>> GetPruebasByGrupoLargadaAsync(Guid grupoLargadaId)
        {
            return await _context.EventoPruebas
                .AsNoTracking()
                .Include(ep => ep.Prueba)
                    .ThenInclude(p => p.Categoria)
                .Include(ep => ep.Prueba)
                    .ThenInclude(p => p.Bote)
                .Include(ep => ep.Prueba)
                    .ThenInclude(p => p.Distancia)
                .Include(ep => ep.Prueba)
                    .ThenInclude(p => p.Sexo)
                .Include(ep => ep.Inscripciones)
                .Where(ep => ep.GrupoLargadaId == grupoLargadaId)
                .OrderBy(ep => ep.FechaHora)
                .ToListAsync();
        }

        public async Task<Entidades.Entidades.Prueba?> GetPruebaAsync(int categoriaId, int boteId, int distanciaId, int sexoId)
        {
            return await _context.Pruebas
                .FirstOrDefaultAsync(p => p.CategoriaEdad == categoriaId && 
                                        p.TipoBote == boteId && 
                                        p.DistanciaId == distanciaId && 
                                        p.SexoCompetencia == sexoId);
        }

        public async Task<Entidades.Entidades.Prueba> CreatePruebaAsync(Entidades.Entidades.Prueba prueba)
        {
            _context.Pruebas.Add(prueba);
            await _context.SaveChangesAsync();
            return prueba;
        }
    }
}
