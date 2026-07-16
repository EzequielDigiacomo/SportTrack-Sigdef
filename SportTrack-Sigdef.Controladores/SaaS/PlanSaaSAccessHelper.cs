using System;
using System.Linq;
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
        private static readonly string[] JudgeRoles = { "Largador", "Cronometrista", "JuezControl" };

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
                ExportacionPdf = plan.ExportacionPdf,
                SoportePrioritario = plan.SoportePrioritario,
                AccesoDashboardClub = plan.AccesoDashboardClub,
                PermitirCargaImagenes = plan.PermitirCargaImagenes
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
            // Jueces solo Ecosistema ST/Dúo (L), no SIGDEF-only
            dto.AccesoControlesLive = (esSportTrack || esPackDuo) && lower.EndsWith("(l)");
        }

        private static (bool sigdef, bool sporttrack, bool live)? ResolveByPlanId(int id)
        {
            if (PackDuoIds.Contains(id))
                return (true, true, id == 9);
            if (SigdefOnlyIds.Contains(id))
                // SIGDEF no tiene consolas juez (aunque sea L)
                return (true, false, false);
            if (SportTrackOnlyIds.Contains(id))
                return (false, true, id == 6);
            return null;
        }

        public static bool CanCreateRole(PlanSaaSDto? plan, string? rol)
        {
            if (string.IsNullOrWhiteSpace(rol)) return false;
            if (plan == null) return false;

            if (string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(rol, "Club", StringComparison.OrdinalIgnoreCase))
                return plan.AccesoDashboardClub;

            if (IsJudgeRole(rol))
                return plan.AccesoControlesLive;

            return true;
        }

        public static bool IsJudgeRole(string? rol) =>
            !string.IsNullOrWhiteSpace(rol) &&
            JudgeRoles.Any(r => string.Equals(r, rol, StringComparison.OrdinalIgnoreCase));
    }
}
