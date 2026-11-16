using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class AtletaFederacion
    {
        [Key]
        public int ParticipanteId { get; set; }

        [ForeignKey(nameof(ParticipanteId))]
        public virtual Participante Participante { get; set; } = null!;

        [ForeignKey("Club")]
        public int? IdClub { get; set; }
        [JsonIgnore]
        public virtual Club? Club { get; set; } = null!;

        public int? IdFederacion { get; set; }
        [ForeignKey(nameof(IdFederacion))]
        public virtual Federacion? Federacion { get; set; }

        public EstadoPago EstadoPago { get; set; }

        public bool PerteneceSeleccion { get; set; }

        public CategoriaEdad? Categoria { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public bool BecadoEnard { get; set; }
        public bool BecadoSdn { get; set; }
        public decimal MontoBeca { get; set; }
        public bool PresentoAptoMedico { get; set; }
        public DateTime? FechaAptoMedico { get; set; }
        public DateTime? FechaVencimientoApto { get; set; }

        /// <summary>Estado del fichaje administrativo (revisión federativa).</summary>
        public EstadoFichaje EstadoFichaje { get; set; } = EstadoFichaje.PendienteRevision;

        public DateTime? FechaEnvioFichaje { get; set; }
        public DateTime? FechaRevision { get; set; }
        public int? RevisadoPorUserId { get; set; }

        [MaxLength(1000)]
        public string? MotivoRevision { get; set; }

        [JsonIgnore]
        public virtual ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
        [JsonIgnore]
        public virtual ICollection<AtletaFederacionTutor> Tutores { get; set; } = new List<AtletaFederacionTutor>();
        [JsonIgnore]
        public virtual ICollection<FichajeRevisionHistorial> HistorialFichaje { get; set; } = new List<FichajeRevisionHistorial>();
    }
}
