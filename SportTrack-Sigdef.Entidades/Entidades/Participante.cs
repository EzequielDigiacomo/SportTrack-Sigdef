using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class Participante
    {
        public int ParticipanteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public int SexoId { get; set; }
        public int? CategoriaId { get; set; }
        public string? Pais { get; set; }
        public int? IdClub { get; set; }
        public Club? Club { get; set; }
        public string? Documento { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public bool PagoAfiliacionAlDia { get; set; } = true;

        // Propiedades calculadas
        public int Edad => DateTime.UtcNow.Year - FechaNacimiento.Year;

        // Navigation properties
        public Sexo Sexo { get; set; } = null!;
        public Categoria? Categoria { get; set; }
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

        // Relaciones de Federación (SIGDEF)
        public virtual DelegadoFederacionClub? DelegadoFederacionClub { get; set; }
        public virtual EntrenadorFederacion? EntrenadorFederacion { get; set; }
        public virtual TutorFederacion? TutorFederacion { get; set; }
        public virtual AtletaFederacion? AtletaFederacion { get; set; }
        public virtual ICollection<DocumentacionFederacionPersona> Documentacion { get; set; } = new List<DocumentacionFederacionPersona>();

        public string? Dni { get => Documento; set => Documento = value; }
        public virtual Usuario? Usuario { get; set; }
        public virtual ICollection<PagoFederacionTransaccion> Pagos { get; set; } = new List<PagoFederacionTransaccion>();
    }
}
