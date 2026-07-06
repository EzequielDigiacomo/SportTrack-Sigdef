using SportTrack_Sigdef.Controladores.SaaS.Dtos;
using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.SaaS
{
    public static class PlanSaaSAccessHelper
    {
        public static PlanSaaSDto FromEntity(PlanSaaS plan)
        {
            var dto = new PlanSaaSDto
            {
                Id = plan.Id,
                Nombre = plan.Nombre,
                Precio = plan.Precio,
                MaxAtletas = plan.MaxAtletas,
                MaxTorneosActivos = plan.MaxTorneosActivos,
                ResultadosTiempoReal = plan.ResultadosTiempoReal,
                ExportacionExcel = plan.ExportacionExcel,
                SoportePrioritario = plan.SoportePrioritario
            };
            ApplyAccessFlags(dto);
            return dto;
        }

        public static void ApplyAccessFlags(PlanSaaSDto dto)
        {
            var nombre = (dto.Nombre ?? string.Empty).Trim();
            var lower = nombre.ToLowerInvariant();

            var esPackDuo = lower.Contains("pack") && (lower.Contains("duo") || lower.Contains("dúo") || lower.Contains("dÃºo"));
            var esSigdef = lower.Contains("sigdef");
            var esSportTrack = lower.Contains("sporttrack");

            dto.AccesoSigdef = esSigdef || esPackDuo;
            dto.AccesoSportTrack = esSportTrack || esPackDuo;
            dto.AccesoControlesLive = lower.EndsWith("(l)");
        }
    }
}
