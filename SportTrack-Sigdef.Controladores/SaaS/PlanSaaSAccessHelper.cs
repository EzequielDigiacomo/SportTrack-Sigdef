using SportTrack_Sigdef.Controladores.SaaS.Dtos;
using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.SaaS
{
    public static class PlanSaaSAccessHelper
    {
        // IDs fijos del seed — no dependen del encoding del nombre en BD
        private static readonly int[] SigdefOnlyIds = { 1, 2, 3 };
        private static readonly int[] SportTrackOnlyIds = { 4, 5, 6 };
        private static readonly int[] PackDuoIds = { 7, 8, 9 };

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
            if (dto == null) return;

            var byId = ResolveByPlanId(dto.Id);
            if (byId.HasValue)
            {
                dto.AccesoSigdef = byId.Value.sigdef;
                dto.AccesoSportTrack = byId.Value.sporttrack;
                dto.AccesoControlesLive = byId.Value.live;
                return;
            }

            var nombre = (dto.Nombre ?? string.Empty).Trim();
            var lower = nombre.ToLowerInvariant();

            var esPackDuo = lower.Contains("pack") && (
                lower.Contains("duo") ||
                lower.Contains("dúo"));
            var esSigdef = lower.Contains("sigdef");
            var esSportTrack = lower.Contains("sporttrack");

            dto.AccesoSigdef = esSigdef || esPackDuo;
            dto.AccesoSportTrack = esSportTrack || esPackDuo;
            dto.AccesoControlesLive = lower.EndsWith("(l)");
        }

        private static (bool sigdef, bool sporttrack, bool live)? ResolveByPlanId(int id)
        {
            if (PackDuoIds.Contains(id))
                return (true, true, id == 9);
            if (SigdefOnlyIds.Contains(id))
                return (true, false, id == 3);
            if (SportTrackOnlyIds.Contains(id))
                return (false, true, id == 6);
            return null;
        }
    }
}
