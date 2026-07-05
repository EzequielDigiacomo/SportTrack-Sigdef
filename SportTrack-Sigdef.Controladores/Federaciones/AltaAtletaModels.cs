using SportTrack_Sigdef.Entidades.Enums;
using System;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    /// <summary>
    /// Datos personales compartidos (Participante) para alta desde SIGDEF o SportTrack.
    /// </summary>
    public class AltaAtletaParticipanteInput
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public int SexoId { get; set; } = 1;
        public int? IdClub { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public int? CategoriaId { get; set; }
        public string? Pais { get; set; }
        public bool PagoAfiliacionAlDia { get; set; } = true;
    }

    /// <summary>
    /// Capa administrativa SIGDEF (AtletaFederacion). SportTrack usa defaults si no las envía.
    /// </summary>
    public class AltaAtletaFederacionInput
    {
        public int? IdClub { get; set; }
        public int? IdFederacion { get; set; }
        public EstadoPago EstadoPago { get; set; } = EstadoPago.Pendiente;
        public bool PerteneceSeleccion { get; set; }
        public CategoriaEdad? Categoria { get; set; }
        public bool BecadoEnard { get; set; }
        public bool BecadoSdn { get; set; }
        public decimal MontoBeca { get; set; }
        public bool PresentoAptoMedico { get; set; }
        public DateTime? FechaAptoMedico { get; set; }
    }

    public class AltaAtletaResult
    {
        public int ParticipanteId { get; set; }
        public Entidades.Entidades.Participante Participante { get; set; } = null!;
        public Entidades.Entidades.AtletaFederacion AtletaFederacion { get; set; } = null!;
        public bool ParticipanteCreado { get; set; }
        public bool AtletaFederacionCreado { get; set; }
    }
}
