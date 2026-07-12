using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Mensajes
{
    public class MensajeRepository : IMensajeRepository
    {
        private readonly SportTrackDbContext _context;

        public MensajeRepository(SportTrackDbContext context)
        {
            _context = context;
        }

        public Task<Usuario?> GetUsuarioByUsernameAsync(string username) =>
            _context.Usuarios
                .Include(u => u.Club)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);

        public Task<Usuario?> GetUsuarioByIdAsync(int id) =>
            _context.Usuarios
                .Include(u => u.Club)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

        public Task<List<Usuario>> GetUsuariosByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.Distinct().ToList();
            return _context.Usuarios
                .Include(u => u.Club)
                .AsNoTracking()
                .Where(u => idList.Contains(u.IdUsuario))
                .ToListAsync();
        }

        public async Task<List<Hilo>> GetHilosVisiblesAsync(int usuarioId, bool esSuperAdmin, string sistemaOrigen, int? campanaId = null)
        {
            var query = _context.Hilos
                .Include(h => h.Mensajes)
                    .ThenInclude(m => m.Remitente)
                .Include(h => h.Mensajes)
                    .ThenInclude(m => m.Destinatario)
                .Where(h => h.SistemaOrigen == sistemaOrigen)
                .AsQueryable();

            if (campanaId.HasValue)
                query = query.Where(h => h.IdCampana == campanaId.Value);

            if (!esSuperAdmin)
            {
                query = query.Where(h => h.Mensajes.Any(m =>
                    (m.RemitenteId == usuarioId && !m.EliminadoPorRemitente) ||
                    (m.DestinatarioId == usuarioId && !m.EliminadoPorDestinatario)));
            }

            return await query
                .OrderByDescending(h => h.UltimoMensajeEn)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Hilo?> GetHiloConMensajesAsync(int hiloId) =>
            _context.Hilos
                .Include(h => h.Mensajes.OrderBy(m => m.EnviadoEn))
                    .ThenInclude(m => m.Remitente)
                .Include(h => h.Mensajes)
                    .ThenInclude(m => m.Destinatario)
                .FirstOrDefaultAsync(h => h.IdHilo == hiloId);

        public Task<bool> UsuarioParticipaEnHiloAsync(int hiloId, int usuarioId) =>
            _context.Mensajes.AnyAsync(m =>
                m.HiloId == hiloId &&
                (m.RemitenteId == usuarioId || m.DestinatarioId == usuarioId));

        public async Task AddHiloAsync(Hilo hilo)
        {
            await _context.Hilos.AddAsync(hilo);
        }

        public async Task AddMensajeAsync(Mensaje mensaje)
        {
            await _context.Mensajes.AddAsync(mensaje);
        }

        public async Task AddCampanaAsync(CampanaEnvio campana)
        {
            await _context.CampanasEnvio.AddAsync(campana);
        }

        public Task<List<CampanaEnvio>> GetCampanasByRemitenteAsync(int remitenteId, string sistemaOrigen) =>
            _context.CampanasEnvio
                .Include(c => c.Hilos)
                    .ThenInclude(h => h.Mensajes)
                .Where(c => c.RemitenteId == remitenteId && c.SistemaOrigen == sistemaOrigen)
                .OrderByDescending(c => c.EnviadoEn)
                .AsNoTracking()
                .ToListAsync();

        public Task<CampanaEnvio?> GetCampanaDetalleAsync(int campanaId) =>
            _context.CampanasEnvio
                .Include(c => c.Hilos)
                    .ThenInclude(h => h.Mensajes)
                        .ThenInclude(m => m.Destinatario)
                .Include(c => c.Hilos)
                    .ThenInclude(h => h.Mensajes)
                        .ThenInclude(m => m.Remitente)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCampana == campanaId);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();

        public Task<int> CountNoLeidosAsync(int usuarioId, string sistemaOrigen) =>
            _context.Mensajes.CountAsync(m =>
                m.DestinatarioId == usuarioId &&
                m.LeidoEn == null &&
                !m.EliminadoPorDestinatario &&
                m.Hilo.SistemaOrigen == sistemaOrigen);
    }
}
