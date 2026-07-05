using System.ComponentModel.DataAnnotations;

namespace SportTrack_Sigdef.Entidades.DTOs.EventoPrueba
{
    public class EventoPruebaUpdateDto : EventoPruebaCreateDto
    {
        [Required]
        public int IdEventoPrueba { get; set; }
    }
}
