using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audience
{
    public sealed class AudienceMetricsService : IAudienceMetricsService
    {
        private readonly IAudiencePresenceTracker _tracker;
        private readonly SportTrackDbContext _db;
        private readonly IConfiguration _configuration;

        public AudienceMetricsService(
            IAudiencePresenceTracker tracker,
            SportTrackDbContext db,
            IConfiguration configuration)
        {
            _tracker = tracker;
            _db = db;
            _configuration = configuration;
        }

        public AudienceLiveDto GetLive()
            => _tracker.GetLiveSnapshot(GetSoftCapacity());

        public async Task<IReadOnlyList<AudiencePeakDto>> GetPeaksAsync(int limit = 100, CancellationToken ct = default)
        {
            var take = Math.Clamp(limit, 1, 500);
            var rows = await _db.AudiencePeakSnapshots
                .AsNoTracking()
                .OrderByDescending(x => x.IsPeakRecord)
                .ThenByDescending(x => x.TotalConnections)
                .ThenByDescending(x => x.CapturedAtUtc)
                .Take(take)
                .ToListAsync(ct);

            return rows.Select(MapPeak).ToList();
        }

        public async Task PersistSnapshotAsync(CancellationToken ct = default)
        {
            var live = GetLive();
            if (live.TotalConnections <= 0)
                return;

            var top = live.ByEvento.FirstOrDefault();
            int? topEventoId = null;
            string? topEventoNombre = null;
            var topConnections = 0;

            if (top != null && int.TryParse(top.EventoId, out var eid))
            {
                topEventoId = eid;
                topConnections = top.Total;
                topEventoNombre = await _db.Eventos
                    .AsNoTracking()
                    .Where(e => e.IdEvento == eid)
                    .Select(e => e.Nombre)
                    .FirstOrDefaultAsync(ct);
            }

            var previousPeak = await _db.AudiencePeakSnapshots
                .AsNoTracking()
                .Where(x => x.IsPeakRecord)
                .OrderByDescending(x => x.TotalConnections)
                .Select(x => x.TotalConnections)
                .FirstOrDefaultAsync(ct);

            var isPeak = live.TotalConnections > previousPeak;

            // Evitar spam: si no es pico nuevo y ya hay snapshot reciente similar, skip.
            if (!isPeak)
            {
                var recent = await _db.AudiencePeakSnapshots
                    .AsNoTracking()
                    .OrderByDescending(x => x.CapturedAtUtc)
                    .FirstOrDefaultAsync(ct);

                if (recent != null
                    && (DateTime.UtcNow - recent.CapturedAtUtc).TotalSeconds < 25
                    && recent.TotalConnections == live.TotalConnections)
                {
                    return;
                }
            }

            _db.AudiencePeakSnapshots.Add(new AudiencePeakSnapshot
            {
                CapturedAtUtc = live.CapturedAtUtc,
                TotalConnections = live.TotalConnections,
                LiveConnections = live.LiveConnections,
                OperatorConnections = live.OperatorConnections,
                SoftCapacity = live.SoftCapacity,
                SaturationPercent = live.SaturationPercent,
                TopEventoId = topEventoId,
                TopEventoNombre = topEventoNombre,
                TopEventoConnections = topConnections,
                IsPeakRecord = isPeak || previousPeak == 0
            });

            await _db.SaveChangesAsync(ct);
        }

        private int GetSoftCapacity()
        {
            var configured = _configuration.GetValue<int?>("AudienceMonitoring:SoftCapacity");
            return configured is > 0 ? configured.Value : 1000;
        }

        private static AudiencePeakDto MapPeak(AudiencePeakSnapshot x) => new()
        {
            Id = x.Id,
            CapturedAtUtc = x.CapturedAtUtc,
            TotalConnections = x.TotalConnections,
            LiveConnections = x.LiveConnections,
            OperatorConnections = x.OperatorConnections,
            SoftCapacity = x.SoftCapacity,
            SaturationPercent = x.SaturationPercent,
            TopEventoId = x.TopEventoId,
            TopEventoNombre = x.TopEventoNombre,
            TopEventoConnections = x.TopEventoConnections,
            IsPeakRecord = x.IsPeakRecord
        };
    }
}
