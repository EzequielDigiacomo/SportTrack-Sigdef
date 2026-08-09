using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    [Table("AudiencePeakSnapshots")]
    public class AudiencePeakSnapshot
    {
        [Key]
        public long Id { get; set; }

        public DateTime CapturedAtUtc { get; set; } = DateTime.UtcNow;

        public int TotalConnections { get; set; }

        public int LiveConnections { get; set; }

        public int OperatorConnections { get; set; }

        public int SoftCapacity { get; set; }

        public double SaturationPercent { get; set; }

        /// <summary>Evento con más conexiones en ese instante (si aplica).</summary>
        public int? TopEventoId { get; set; }

        [MaxLength(200)]
        public string? TopEventoNombre { get; set; }

        public int TopEventoConnections { get; set; }

        public bool IsPeakRecord { get; set; }
    }
}
