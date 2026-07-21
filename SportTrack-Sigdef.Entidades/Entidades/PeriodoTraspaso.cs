using System;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class PeriodoTraspaso
    {
        public int IdPeriodoTraspaso { get; set; }
        public int IdFederacion { get; set; }
        public Federacion Federacion { get; set; } = null!;

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; } = true;
        public string? Observaciones { get; set; }

        public int? CreadoPorUsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
