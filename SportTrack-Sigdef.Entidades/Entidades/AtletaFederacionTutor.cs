using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class AtletaFederacionTutor
    {
        [Key, Column(Order = 0)]
        public int IdAtleta { get; set; }

        [Key, Column(Order = 1)]
        public int IdTutor { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(IdAtleta))]
        public virtual AtletaFederacion AtletaFederacion { get; set; } = null!;
        [JsonIgnore]
        [ForeignKey(nameof(IdTutor))]
        public virtual TutorFederacion TutorFederacion { get; set; } = null!;

        public Parentesco Parentesco { get; set; }
    }
}
