namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class CampanaEnvio
    {
        public int IdCampana { get; set; }
        public int RemitenteId { get; set; }
        public Usuario? Remitente { get; set; }
        public string Asunto { get; set; } = string.Empty;
        public string Cuerpo { get; set; } = string.Empty;
        public DateTime EnviadoEn { get; set; } = DateTime.UtcNow;
        public int CantidadDestinatarios { get; set; }
        public string TipoCampana { get; set; } = string.Empty; // SuperAdminMasivo | AdminMasivo
        /// <summary>Producto origen: sporttrack | sigdef</summary>
        public string SistemaOrigen { get; set; } = MensajeriaSistemaOrigen.SportTrack;

        public ICollection<Hilo> Hilos { get; set; } = new List<Hilo>();
    }
}
