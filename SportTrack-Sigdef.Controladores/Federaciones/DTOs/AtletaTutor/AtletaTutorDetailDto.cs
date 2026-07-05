using SIGDEF.DTOs;
using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;
using SportTrack_Sigdef.Entidades.Enums;

namespace SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor
{
    public class AtletaTutorDetailDto
    {
        public int ParticipanteId { get; set; }
        public int IdTutor { get; set; }
        public Parentesco Parentesco { get; set; }
        public AtletaDto? AtletaFederacion { get; set; }
        public TutorDto? TutorFederacion { get; set; }
    }
}
