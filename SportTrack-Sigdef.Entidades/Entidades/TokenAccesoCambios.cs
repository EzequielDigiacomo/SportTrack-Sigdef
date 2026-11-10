using System;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    /// <summary>
    /// Link mágico para que un club corrija tripulaciones de un evento sin login JWT.
    /// </summary>
    public class TokenAccesoCambios
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public int IdEvento { get; set; }
        public int IdClub { get; set; }
        public DateTime ExpiraEn { get; set; }
        public bool Revocado { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string? CreadoPor { get; set; }
        public int Usos { get; set; }

        public Evento Evento { get; set; } = null!;
        public Club Club { get; set; } = null!;
    }
}
