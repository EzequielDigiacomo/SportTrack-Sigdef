using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Evento
{
    /// <summary>
    /// Sincroniza periódicamente Programada → EnCurso → Finalizado según fechas del evento.
    /// </summary>
    public class EventoEstadoBackgroundService : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(15);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EventoEstadoBackgroundService> _logger;

        public EventoEstadoBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<EventoEstadoBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Pequeña demora para no competir con el arranque/migraciones.
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var syncService = scope.ServiceProvider.GetRequiredService<IEventoEstadoSyncService>();
                    await syncService.SyncAllAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al sincronizar estados de eventos en background.");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }
    }
}
