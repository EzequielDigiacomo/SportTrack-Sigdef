using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Distancia.Dtos
{
    public class DistanciaUpdateDto
    {
        [Required]
        public DistanciaRegataEnum DistanciaRegata { get; set; }
        public int GapSugerido { get; set; }
    }
}
