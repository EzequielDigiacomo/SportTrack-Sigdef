using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    /// <summary>Configuración de control de saturación (no limita conexiones).</summary>
    [Table("AudienceMonitorSettings")]
    public class AudienceMonitorSettings
    {
        [Key]
        public int Id { get; set; } = 1;

        public int SoftCapacity { get; set; } = 200;

        [MaxLength(40)]
        public string PresetId { get; set; } = "starter";

        [MaxLength(120)]
        public string PlanLabel { get; set; } = "API Starter + DB Basic-1gb";

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
