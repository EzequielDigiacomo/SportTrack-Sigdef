using System.Collections.Generic;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class PlanSaaS
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        /// <summary>Descuento % aplicado al precio anual (sobre 12× mensual).</summary>
        public decimal DescuentoAnualPorcentaje { get; set; }
        /// <summary>Precio anual calculado: Precio × 12 × (1 − DescuentoAnualPorcentaje/100).</summary>
        public decimal PrecioAnual { get; set; }

        // Limites
        public int MaxAtletas { get; set; } // -1 para ilimitado
        public int MaxTorneosActivos { get; set; } // -1 para ilimitado

        // Features
        public bool ResultadosTiempoReal { get; set; }
        public bool ExportacionExcel { get; set; }
        public bool ExportacionPdf { get; set; }
        public bool SoportePrioritario { get; set; }
        public bool AccesoDashboardClub { get; set; }
        public bool PermitirCargaImagenes { get; set; }

        // Navegación
        public ICollection<Club> Clubes { get; set; } = new List<Club>();
        public ICollection<Federacion> Federaciones { get; set; } = new List<Federacion>();
    }
}
