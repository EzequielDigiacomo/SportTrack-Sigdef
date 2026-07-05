using SportTrack_Sigdef.Entidades.Enums;
using System.ComponentModel.DataAnnotations;

namespace SportTrack_Sigdef.Entidades.DTOs.EventoPrueba
{
    public class EventoPruebaCreateDto
    {
       public int IdEvento { get; set; }
        [Required(ErrorMessage = "La distancia es requerida")]
        public int IdPrueba { get; set; }
        
        public decimal? PrecioCategoria { get; set; }
    }
}
