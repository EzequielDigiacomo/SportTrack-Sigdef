using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class TutorFederacion
    {
        [Key]
        [ForeignKey(nameof(Participante))]
        public int ParticipanteId { get; set; }

        public virtual Participante Participante { get; set; } = null!;

        [MaxLength(50)]
        public string TipoTutor { get; set; } = string.Empty;

        public virtual ICollection<AtletaFederacionTutor> AtletasTutores { get; set; } = new List<AtletaFederacionTutor>();
    }
}
