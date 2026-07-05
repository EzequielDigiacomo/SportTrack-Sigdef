using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub
{
    public class DelegadoClubCreateDto
    {
        public int? ParticipanteId { get; set; }
        public int IdRol { get; set; }

        public int? IdFederacion { get; set; }

        public int? IdClub { get; set; }
    }

}
