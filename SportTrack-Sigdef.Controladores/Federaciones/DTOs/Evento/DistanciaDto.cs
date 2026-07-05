using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.DTOs.Evento
{
    public class DistanciaDTO
    {
        public DistanciaRegata DistanciaRegata { get; set; }
        public int CategoriaEdad { get; set; }
        public int TipoBote { get; set; }
        public int SexoCompetencia { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
