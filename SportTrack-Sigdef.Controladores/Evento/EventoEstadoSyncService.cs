using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Evento
{
    public class EventoEstadoSyncService : IEventoEstadoSyncService
    {
        private readonly SportTrackDbContext _context;
        private readonly ILogger<EventoEstadoSyncService> _logger;

        public EventoEstadoSyncService(SportTrackDbContext context, ILogger<EventoEstadoSyncService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> SyncAllAsync(CancellationToken cancellationToken = default)
        {
            var utcNow = DateTime.UtcNow;
            var eventos = await _context.Eventos
                .Where(e => e.Estado == EstadoEventoEnum.Programada || e.Estado == EstadoEventoEnum.EnCurso)
                .ToListAsync(cancellationToken);

            var updated = 0;
            foreach (var evento in eventos)
            {
                if (ApplyEstadoIfChanged(evento, utcNow))
                    updated++;
            }

            if (updated > 0)
            {
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Estados de evento sincronizados automáticamente: {Count} actualizado(s).", updated);
            }

            return updated;
        }

        public async Task<bool> SyncEventoAsync(int eventoId, CancellationToken cancellationToken = default)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.IdEvento == eventoId, cancellationToken);
            if (evento == null || !EventoEstadoSyncHelper.ShouldAutoSync(evento.Estado))
                return false;

            if (!ApplyEstadoIfChanged(evento, DateTime.UtcNow))
                return false;

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation(
                "Evento {EventoId} ({Nombre}) actualizado a {Estado}.",
                evento.IdEvento,
                evento.Nombre,
                evento.Estado);

            return true;
        }

        private static bool ApplyEstadoIfChanged(Entidades.Entidades.Evento evento, DateTime utcNow)
        {
            var nuevoEstado = EventoEstadoSyncHelper.ComputeEstado(evento, utcNow);
            if (nuevoEstado == evento.Estado)
                return false;

            // Solo avanzar automáticamente; no retroceder si un admin adelantó el estado manualmente.
            if (nuevoEstado < evento.Estado)
                return false;

            evento.Estado = nuevoEstado;
            evento.FechaActualizacion = utcNow;
            return true;
        }
    }
}
