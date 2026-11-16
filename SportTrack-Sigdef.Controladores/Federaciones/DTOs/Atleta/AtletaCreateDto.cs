using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SIGDEF.DTOs
{
    public class AtletaCreateDto
    {
        [Required]
        public int ParticipanteId { get; set; }
        public int? IdClub { get; set; }
        public int? IdFederacion { get; set; }

        [Required]
        public EstadoPago EstadoPago { get; set; }

        [Required]
        public bool PerteneceSeleccion { get; set; }

        public CategoriaEdad? Categoria { get; set; }

        [Required]
        public bool BecadoEnard { get; set; }

        [Required]
        public bool BecadoSdn { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El monto de la beca debe ser mayor o igual a 0")]
        public decimal MontoBeca { get; set; }

        [Required]
        public bool PresentoAptoMedico { get; set; }

        public DateTime? FechaAptoMedico { get; set; }
        public DateTime? FechaVencimientoApto { get; set; }
        public DateTime FechaCreacion { get; set; }

        /// <summary>Si null, el servicio asigna según rol (Club=Pendiente, Admin=Aprobado).</summary>
        public EstadoFichaje? EstadoFichaje { get; set; }

        public string? NombrePersona { get; set; }
        public string? NombreClub { get; set; }
    }
}
