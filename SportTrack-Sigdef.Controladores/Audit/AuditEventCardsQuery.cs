using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Audit;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audit
{
    public static class AuditEventCardsQuery
    {
        private static readonly string[] EventRelatedModules =
        {
            "Competencia", "Inscripciones", "Eventos", "Frontend", "Cronometrista", "Largador", "JuezControl", "Resultados",
        };

        public static bool IsEventRelatedLog(Auditoria log)
        {
            if (log.IdEvento.HasValue) return true;
            var modulo = log.Modulo ?? string.Empty;
            var accion = log.Accion ?? string.Empty;
            if (EventRelatedModules.Contains(modulo, StringComparer.OrdinalIgnoreCase)) return true;
            if (accion.StartsWith("CLICK_", StringComparison.OrdinalIgnoreCase)) return true;
            if (accion.StartsWith("OPEN_", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        public static async Task<List<object>> BuildCardsAsync(
            SportTrackDbContext context,
            IQueryable<Auditoria> scoped,
            int eventosLimit,
            int logsPerEvento)
        {
            eventosLimit = Math.Clamp(eventosLimit, 1, 50);
            logsPerEvento = Math.Clamp(logsPerEvento, 1, 30);

            var allRecent = await scoped
                .AsNoTracking()
                .OrderByDescending(a => a.Fecha)
                .Take(2500)
                .ToListAsync();

            var relevant = allRecent.Where(IsEventRelatedLog).ToList();
            var withEvento = relevant.Where(a => a.IdEvento.HasValue).ToList();
            var withoutEvento = relevant.Where(a => !a.IdEvento.HasValue).ToList();

            var legacyGroups = await AuditLegacyScopeResolver.GroupLegacyLogsByEventoAsync(context, withoutEvento);

            var groupedMap = withEvento
                .GroupBy(a => a.IdEvento!.Value)
                .ToDictionary(
                    g => g.Key,
                    g => new GroupRow
                    {
                        EventoId = g.Key,
                        UltimaActividad = g.Max(a => a.Fecha),
                        Total = g.Count(),
                        DirectLogs = g.ToList(),
                    });

            foreach (var kv in legacyGroups)
            {
                if (kv.Value.Count == 0) continue;
                var ultima = kv.Value.Max(l => l.Fecha);
                if (!groupedMap.TryGetValue(kv.Key, out var row))
                {
                    groupedMap[kv.Key] = new GroupRow
                    {
                        EventoId = kv.Key,
                        UltimaActividad = ultima,
                        Total = kv.Value.Count,
                        LegacyLogs = kv.Value,
                    };
                }
                else
                {
                    row.Total += kv.Value.Count;
                    if (ultima > row.UltimaActividad) row.UltimaActividad = ultima;
                    row.LegacyLogs.AddRange(kv.Value);
                }
            }

            var top = groupedMap.Values
                .OrderByDescending(x => x.UltimaActividad)
                .Take(eventosLimit)
                .ToList();

            if (top.Count == 0) return new List<object>();

            var ids = top.Select(x => x.EventoId).ToList();
            var eventos = await context.Eventos
                .AsNoTracking()
                .Where(e => ids.Contains(e.IdEvento))
                .Select(e => new { e.IdEvento, e.Nombre, e.Estado, e.Fecha })
                .ToDictionaryAsync(e => e.IdEvento);

            var cards = new List<object>();
            foreach (var row in top)
            {
                eventos.TryGetValue(row.EventoId, out var ev);
                var logs = row.DirectLogs
                    .Concat(row.LegacyLogs)
                    .OrderByDescending(l => l.Fecha)
                    .Take(logsPerEvento)
                    .Select(MapLogRow)
                    .ToList();

                cards.Add(new
                {
                    eventoId = row.EventoId,
                    eventoNombre = ev?.Nombre ?? $"Evento #{row.EventoId}",
                    eventoEstado = ev?.Estado.ToString(),
                    eventoFecha = ev?.Fecha,
                    ultimaActividad = row.UltimaActividad,
                    totalRegistros = row.Total,
                    logs,
                });
            }

            return cards;
        }

        private static object MapLogRow(Auditoria a) => new
        {
            id = a.Id,
            fecha = a.Fecha,
            accion = a.Accion,
            detalle = a.Detalle,
            usuario = a.Usuario,
            modulo = a.Modulo,
            ip = a.IP,
            idEvento = a.IdEvento,
            idEventoPrueba = a.IdEventoPrueba,
        };

        private sealed class GroupRow
        {
            public int EventoId { get; set; }
            public DateTime UltimaActividad { get; set; }
            public int Total { get; set; }
            public List<Auditoria> DirectLogs { get; set; } = new();
            public List<Auditoria> LegacyLogs { get; set; } = new();
        }
    }
}
