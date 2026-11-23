using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audit
{
    /// <summary>
    /// Purga automática diaria de auditoría según la config de SuperAdmin.
    /// </summary>
    public class AuditRetentionBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AuditRetentionBackgroundService> _logger;

        public AuditRetentionBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<AuditRetentionBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Esperar un poco al arrancar para no competir con migraciones / warmup.
            try { await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken); }
            catch (OperationCanceledException) { return; }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var retention = scope.ServiceProvider.GetRequiredService<IAuditRetentionService>();
                    await retention.RunAutomaticPurgesAsync(stoppingToken);
                    _logger.LogInformation("Auditoría: ciclo de purga automática ejecutado.");
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogWarning(ex, "Error en purga automática de auditoría.");
                }

                try { await Task.Delay(TimeSpan.FromHours(24), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }
}
