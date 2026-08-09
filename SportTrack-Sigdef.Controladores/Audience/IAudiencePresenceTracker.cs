using System.Collections.Generic;

namespace SportTrack_Sigdef.Controladores.Audience
{
    public interface IAudiencePresenceTracker
    {
        void Upsert(string connectionId, string? eventoId, string? faseId, string? userName, string? role);
        void MarkOperator(string connectionId);
        void Remove(string connectionId);
        AudienceLiveDto GetLiveSnapshot(int softCapacity);
        IReadOnlyCollection<AudienceConnectionState> GetConnections();
    }
}
