using SportTrack_Sigdef.Entidades.Enums;
using System;
using SportTrack_Sigdef.Entidades.DTOs.Club;
using SportTrack_Sigdef.Entidades.DTOs.Participante;
using SportTrack_Sigdef.Entidades.DTOs.Inscripcion;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion
{
    public class AtletaDetailDto
    {
        public int ParticipanteId { get; set; }
        public int IdPersona => ParticipanteId;
        public int? IdClub { get; set; } = null;
        public EstadoPago EstadoPago { get; set; }
        public bool PerteneceSeleccion { get; set; }
        public CategoriaEdad? Categoria { get; set; }
        public bool BecadoEnard { get; set; }
        public bool BecadoSdn { get; set; }
        public decimal MontoBeca { get; set; }
        public bool PresentoAptoMedico { get; set; }
        public DateTime? FechaAptoMedico { get; set; }
         public DateTime FechaCreacion { get; set; }
        // Información relacionada
        public PersonaDto? Participante { get; set; }
        public ClubDto? Club { get; set; }
        public List<InscripcionDto>? Inscripciones { get; set; }
        public List<AtletaTutorDto>? Tutores { get; set; }
    }
}
