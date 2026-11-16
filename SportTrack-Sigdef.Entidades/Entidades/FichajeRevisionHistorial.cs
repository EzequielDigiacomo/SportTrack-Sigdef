using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SportTrack_Sigdef.Entidades.Enums;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class FichajeRevisionHistorial
    {
        [Key]
        public int Id { get; set; }

        public int ParticipanteId { get; set; }

        [ForeignKey(nameof(ParticipanteId))]
        public virtual AtletaFederacion Atleta { get; set; } = null!;

        public EstadoFichaje EstadoAnterior { get; set; }
        public EstadoFichaje EstadoNuevo { get; set; }

        [MaxLength(1000)]
        public string? Motivo { get; set; }

        public int? RevisadoPorUserId { get; set; }

        [MaxLength(100)]
        public string? RevisadoPorUsername { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
