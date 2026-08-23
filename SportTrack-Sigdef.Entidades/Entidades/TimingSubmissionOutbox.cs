using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    /// <summary>
    /// Cola temporal de envíos de tiempos del cronometrista cuando falla la red.
    /// Se borra al confirmar en la tabla Resultados.
    /// </summary>
    [Table("TimingSubmissionOutbox")]
    public class TimingSubmissionOutbox
    {
        [Key]
        public int Id { get; set; }

        public int FaseId { get; set; }

        public int? EventoId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PayloadJson { get; set; } = "{}";

        public bool SoloMode { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAtUtc { get; set; }

        public int AttemptCount { get; set; }

        public DateTime? LastAttemptAtUtc { get; set; }
    }
}
