using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor
{
    public class AtletaTutorCreateDto
    {
        public int ParticipanteId { get; set; }
        public int IdTutor { get; set; }
        public Parentesco Parentesco { get; set; }
    }
}
