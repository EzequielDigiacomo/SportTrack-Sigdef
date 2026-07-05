using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class RolFederacion
    {
        [Key]
        public int IdRol { get; set; }

        [Required, MaxLength(50)]
        public string Tipo { get; set; } = string.Empty;

        [NotMapped]
        public string TipoNombre => Tipo;

        [NotMapped]
        public RolTipo? TipoEnum
        {
            get
            {
                if (Enum.TryParse<RolTipo>(Tipo, out var tipoEnum))
                    return tipoEnum;
                return null;
            }
        }

        public virtual ICollection<DelegadoFederacionClub> DelegadosClub { get; set; } = new List<DelegadoFederacionClub>();
    }
}
