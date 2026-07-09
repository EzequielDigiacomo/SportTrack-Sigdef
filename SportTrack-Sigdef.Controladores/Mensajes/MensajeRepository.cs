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
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);

        public Task<Usuario?> GetUsuarioByIdAsync(int id) =>
            _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

        public async Task<List<Hilo>> GetHilosVisiblesAsync(int usuarioId, bool esSuperAdmin)
        {
            var query = _context.Hilos
                .Include(h => h.Mensajes)
                    .ThenInclude(m => m.Remitente)
                .Include(h => h.Mensajes)
                    .ThenInclude(m => m.Destinatario)
                .AsQueryable();

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

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
