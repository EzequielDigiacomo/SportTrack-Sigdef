using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audience
{
    /// <summary>
    /// Persiste snapshots de audiencia periódicamente (solo lectura del tracker + insert).
    /// </summary>
    public sealed class AudienceSnapshotBackgroundService : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(30);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AudienceSnapshotBackgroundService> _logger;

        public AudienceSnapshotBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<AudienceSnapshotBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(45), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var metrics = scope.ServiceProvider.GetRequiredService<IAudienceMetricsService>();
                    await metrics.PersistSnapshotAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "No se pudo persistir snapshot de audiencia.");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }
    }
}
