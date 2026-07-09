namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class Mensaje
    {
        public int IdMensaje { get; set; }
        public int HiloId { get; set; }
        public Hilo? Hilo { get; set; }
        public int RemitenteId { get; set; }
        public Usuario? Remitente { get; set; }
        public int DestinatarioId { get; set; }
        public Usuario? Destinatario { get; set; }
        public string Cuerpo { get; set; } = string.Empty;
        public DateTime EnviadoEn { get; set; } = DateTime.UtcNow;
        public DateTime? LeidoEn { get; set; }
        public bool EliminadoPorRemitente { get; set; }
        public bool EliminadoPorDestinatario { get; set; }
    }
}
