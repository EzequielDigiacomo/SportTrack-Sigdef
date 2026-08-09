using System.Collections.Generic;

namespace SportTrack_Sigdef.Controladores.Audience
{
    public sealed class AudienceCapacityPresetDto
    {
        public string Id { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Hint { get; set; } = string.Empty;
        public int SoftCapacity { get; set; }
        public bool IsCustom { get; set; }
    }

    public sealed class AudienceCapacityConfigDto
    {
        public int SoftCapacity { get; set; }
        public string PresetId { get; set; } = "starter";
        public string PlanLabel { get; set; } = string.Empty;
        public List<AudienceCapacityPresetDto> Presets { get; set; } = new();
        public string Note { get; set; } =
            "Solo control visual de saturación. No limita ni corta conexiones.";
    }

    public sealed class AudienceCapacityUpdateRequest
    {
        public string? PresetId { get; set; }
        public int? SoftCapacity { get; set; }
    }
}
