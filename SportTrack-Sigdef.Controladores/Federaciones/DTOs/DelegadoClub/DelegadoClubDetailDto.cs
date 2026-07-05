using SportTrack_Sigdef.Entidades.DTOs.Club;
using SportTrack_Sigdef.Entidades.DTOs.Federacion;
using SportTrack_Sigdef.Entidades.DTOs.Participante;
using SportTrack_Sigdef.Entidades.DTOs.RolFederacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub
{
    public class DelegadoClubDetailDto
    {
        public int? ParticipanteId { get; set; }
        public int IdRol { get; set; }
        public int? IdFederacion { get; set; }
        public int? IdClub { get; set; }

        public PersonaDto? Participante { get; set; }
        public RolDto? RolFederacion { get; set; }
        public FederacionDto? Federacion { get; set; }
        public ClubDto? Club { get; set; }
    }
}
