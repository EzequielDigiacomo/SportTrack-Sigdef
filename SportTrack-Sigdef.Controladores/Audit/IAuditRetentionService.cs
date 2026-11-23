using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audit
{
    public interface IAuditRetentionService
    {
        Task<AuditRetentionOptions> GetAsync(CancellationToken ct = default);
        Task<AuditRetentionOptions> SaveAsync(AuditRetentionOptions options, CancellationToken ct = default);
        Task<FederacionAuditRetentionOptions> UpsertFederacionAsync(FederacionAuditRetentionOptions options, CancellationToken ct = default);
        Task<int> PurgeOlderThanAsync(int days, CancellationToken ct = default);
        Task<int> PurgeFederacionAsync(FederacionAuditPurgeRequest request, CancellationToken ct = default);
        Task RunAutomaticPurgesAsync(CancellationToken ct = default);
    }
}
