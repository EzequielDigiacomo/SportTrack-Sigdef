using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SportTrack_Sigdef.Entidades.Enums;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class DocumentacionFederacionPersona
    {
        [Key]
        public int Id { get; set; }
        public int? PersonaId { get; set; }

        [ForeignKey("PersonaId")]
        public virtual Participante Participante { get; set; } = null!;

        /// <summary>Ver <see cref="TipoDocumentoFederacion"/>.</summary>
        public int? TipoDocumento { get; set; }

        [Required]
        public string UrlArchivo { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? PublicId { get; set; }

        public DateTime FechaCarga { get; set; } = DateTime.UtcNow;

        public DateTime? FechaVencimiento { get; set; }

        public EstadoRevisionDocumento EstadoRevision { get; set; } = EstadoRevisionDocumento.Pendiente;

        [MaxLength(500)]
        public string? Observacion { get; set; }
    }
}
