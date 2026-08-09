using System.ComponentModel.DataAnnotations;

namespace SportTrack_Sigdef.Controladores.SaaS.Dtos
{
    public class PlanSaaSUpdateDto
    {
        [Range(0, 999999)]
        public decimal Precio { get; set; }

        /// <summary>Descuento % sobre 12× mensual para el plan anual (0–100).</summary>
        [Range(0, 100)]
        public decimal DescuentoAnualPorcentaje { get; set; }

        /// <summary>-1 = ilimitado</summary>
        [Range(-1, int.MaxValue)]
        public int MaxAtletas { get; set; }
    }
}
