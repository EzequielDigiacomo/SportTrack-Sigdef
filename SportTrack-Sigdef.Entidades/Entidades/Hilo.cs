namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class Hilo
    {
        public int IdHilo { get; set; }
        public string Asunto { get; set; } = string.Empty;
        public int? IdCampana { get; set; }
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
        public DateTime UltimoMensajeEn { get; set; } = DateTime.UtcNow;

        public ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
    }
}
