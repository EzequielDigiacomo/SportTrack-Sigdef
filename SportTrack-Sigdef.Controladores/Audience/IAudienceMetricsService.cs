using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audience
{
    public interface IAudienceMetricsService
    {
        Task EnsureReadyAsync(CancellationToken ct = default);
        AudienceLiveDto GetLive();
        Task<IReadOnlyList<AudiencePeakDto>> GetPeaksAsync(int limit = 100, CancellationToken ct = default);
        Task PersistSnapshotAsync(CancellationToken ct = default);
        AudienceCapacityConfigDto GetCapacityConfig();
        Task ApplyCapacityAsync(AudienceCapacityUpdateRequest request, CancellationToken ct = default);
    }
}
