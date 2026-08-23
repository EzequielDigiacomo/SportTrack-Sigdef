using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audit
{
    /// <summary>
    /// Vincula registros viejos de Auditoria (sin IdEvento) al evento correspondiente.
    /// </summary>
    public static class AuditLegacyScopeResolver
    {
        private static readonly Regex FaseIdPattern = new(@"\(ID:\s*(\d+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex PruebaIdPattern = new(@"Prueba ID\s*(\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static async Task<Dictionary<int, List<Auditoria>>> GroupLegacyLogsByEventoAsync(
            SportTrackDbContext context,
            IReadOnlyList<Auditoria> legacyLogs)
        {
            var result = new Dictionary<int, List<Auditoria>>();
            if (legacyLogs == null || legacyLogs.Count == 0) return result;

            var faseIds = new HashSet<int>();
            var pruebaIds = new HashSet<int>();

            foreach (var log in legacyLogs)
            {
                var detalle = log.Detalle ?? string.Empty;
                var faseMatch = FaseIdPattern.Match(detalle);
                if (faseMatch.Success && int.TryParse(faseMatch.Groups[1].Value, out var faseId))
                    faseIds.Add(faseId);

                var pruebaMatch = PruebaIdPattern.Match(detalle);
                if (pruebaMatch.Success && int.TryParse(pruebaMatch.Groups[1].Value, out var pruebaId))
                    pruebaIds.Add(pruebaId);
            }

            var faseScope = faseIds.Count == 0
                ? new Dictionary<int, (int EventoId, int? EventoPruebaId)>()
                : await context.Fases
                    .AsNoTracking()
                    .Where(f => faseIds.Contains(f.Id))
                    .Select(f => new
                    {
                        f.Id,
                        EventoId = f.Etapa.EventoPrueba.IdEvento,
                        EventoPruebaId = (int?)f.Etapa.EventoPruebaId,
                    })
                    .ToDictionaryAsync(x => x.Id, x => (x.EventoId, x.EventoPruebaId));

            var pruebaScope = pruebaIds.Count == 0
                ? new Dictionary<int, int>()
                : await context.EventoPruebas
                    .AsNoTracking()
                    .Where(ep => pruebaIds.Contains(ep.IdEventoPrueba))
                    .ToDictionaryAsync(ep => ep.IdEventoPrueba, ep => ep.IdEvento);

            foreach (var log in legacyLogs)
            {
                var detalle = log.Detalle ?? string.Empty;
                int? eventoId = AuditScopeDetalle.TryExtract(detalle).EventoId;

                if (!eventoId.HasValue)
                {
                    var faseMatch = FaseIdPattern.Match(detalle);
                    if (faseMatch.Success
                        && int.TryParse(faseMatch.Groups[1].Value, out var faseId)
                        && faseScope.TryGetValue(faseId, out var scope))
                    {
                        eventoId = scope.EventoId;
                    }
                }

                if (!eventoId.HasValue)
                {
                    var pruebaMatch = PruebaIdPattern.Match(detalle);
                    if (pruebaMatch.Success
                        && int.TryParse(pruebaMatch.Groups[1].Value, out var pruebaId)
                        && pruebaScope.TryGetValue(pruebaId, out var evId))
                    {
                        eventoId = evId;
                    }
                }

                if (!eventoId.HasValue) continue;

                if (!result.TryGetValue(eventoId.Value, out var list))
                {
                    list = new List<Auditoria>();
                    result[eventoId.Value] = list;
                }
                list.Add(log);
            }

            return result;
        }
    }
}
