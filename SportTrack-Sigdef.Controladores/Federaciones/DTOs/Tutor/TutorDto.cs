using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.DTOs.TutorFederacion
{
    public class TutorDto
    {
        public int ParticipanteId { get; set; }
        public int IdPersona => ParticipanteId;
        public string TipoTutor { get; set; } = string.Empty;

        // Información adicional para mostrar
        public string? NombrePersona { get; set; }
        public string? Documento { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public int CantidadAtletas { get; set; }
    }
}
