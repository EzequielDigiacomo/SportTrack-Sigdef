namespace SportTrack_Sigdef.Entidades.DTOs.EventoPrueba
{
    public class EventoPruebaDto
    {
        public int IdEventoPrueba { get; set; }
        public int IdEvento { get; set; }
        public int IdPrueba { get; set; }
        public decimal? PrecioCategoria { get; set; }
        
        public SportTrack_Sigdef.Entidades.DTOs.Prueba.PruebaDto Prueba { get; set; }
    }
}
