using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audience
{
    public interface IAudienceCapacitySettings
    {
        int SoftCapacity { get; }
        string PresetId { get; }
        string PlanLabel { get; }
        IReadOnlyList<AudienceCapacityPresetDto> GetPresets();
        AudienceCapacityConfigDto GetConfig();
        Task EnsureLoadedAsync(CancellationToken ct = default);
        Task ApplyAsync(AudienceCapacityUpdateRequest request, CancellationToken ct = default);
    }
}
