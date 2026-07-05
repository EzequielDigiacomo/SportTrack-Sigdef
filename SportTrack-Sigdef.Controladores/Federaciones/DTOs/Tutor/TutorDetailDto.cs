using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;
using SportTrack_Sigdef.Entidades.DTOs.Participante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.DTOs.TutorFederacion
{
    public class TutorDetailDto
    {
        public int ParticipanteId { get; set; }
        public int IdPersona => ParticipanteId;
        public string TipoTutor { get; set; } = string.Empty;

        public PersonaDto? Participante { get; set; }
        public List<AtletaTutorDto>? AtletasTutores { get; set; }
    }
}
