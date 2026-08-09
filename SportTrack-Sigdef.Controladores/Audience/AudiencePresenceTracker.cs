using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SportTrack_Sigdef.Controladores.Audience
{
    /// <summary>
    /// Contador en memoria de conexiones SignalR (una entrada por ConnectionId).
    /// No altera grupos ni broadcasts del TimingHub.
    /// </summary>
    public sealed class AudiencePresenceTracker : IAudiencePresenceTracker
    {
        private readonly ConcurrentDictionary<string, AudienceConnectionState> _connections = new(StringComparer.Ordinal);

        private int _sessionPeakTotal;
        private DateTime? _sessionPeakAtUtc;
        private readonly object _peakLock = new();

        public void Upsert(string connectionId, string? eventoId, string? faseId, string? userName, string? role)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                return;

            var isOperator = IsOperatorRole(role);
            _connections.AddOrUpdate(
                connectionId,
                _ =>
                {
                    var created = new AudienceConnectionState
                    {
                        ConnectionId = connectionId,
                        EventoId = Normalize(eventoId),
                        FaseId = Normalize(faseId),
                        UserName = userName,
                        Role = role,
                        IsOperator = isOperator,
                        ConnectedAtUtc = DateTime.UtcNow,
                        LastSeenUtc = DateTime.UtcNow
                    };
                    TouchSessionPeak();
                    return created;
                },
                (_, existing) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventoId))
                        existing.EventoId = Normalize(eventoId);
                    if (!string.IsNullOrWhiteSpace(faseId))
                        existing.FaseId = Normalize(faseId);
                    if (!string.IsNullOrWhiteSpace(userName))
                        existing.UserName = userName;
                    if (!string.IsNullOrWhiteSpace(role))
                    {
                        existing.Role = role;
                        existing.IsOperator = IsOperatorRole(role);
                    }
                    existing.LastSeenUtc = DateTime.UtcNow;
                    TouchSessionPeak();
                    return existing;
                });
        }

        public void MarkOperator(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                return;

            _connections.AddOrUpdate(
                connectionId,
                _ => new AudienceConnectionState
                {
                    ConnectionId = connectionId,
                    IsOperator = true,
                    Role = "Operator",
                    ConnectedAtUtc = DateTime.UtcNow,
                    LastSeenUtc = DateTime.UtcNow
                },
                (_, existing) =>
                {
                    existing.IsOperator = true;
                    if (string.IsNullOrWhiteSpace(existing.Role) || !IsOperatorRole(existing.Role))
                        existing.Role = "Operator";
                    existing.LastSeenUtc = DateTime.UtcNow;
                    return existing;
                });

            TouchSessionPeak();
        }

        public void Remove(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                return;
            _connections.TryRemove(connectionId, out _);
        }

        public IReadOnlyCollection<AudienceConnectionState> GetConnections()
            => _connections.Values.ToList();

        public AudienceLiveDto GetLiveSnapshot(int softCapacity)
        {
            var capacity = softCapacity <= 0 ? 1000 : softCapacity;
            var list = _connections.Values.ToList();
            var total = list.Count;
            var operators = list.Count(c => c.IsOperator);
            var live = Math.Max(0, total - operators);
            var percent = capacity <= 0 ? 0 : Math.Round(100.0 * total / capacity, 1);

            int sessionPeak;
            DateTime? sessionPeakAt;
            lock (_peakLock)
            {
                if (total > _sessionPeakTotal)
                {
                    _sessionPeakTotal = total;
                    _sessionPeakAtUtc = DateTime.UtcNow;
                }
                sessionPeak = _sessionPeakTotal;
                sessionPeakAt = _sessionPeakAtUtc;
            }

            var byEvent = list
                .Where(c => !string.IsNullOrWhiteSpace(c.EventoId))
                .GroupBy(c => c.EventoId!)
                .Select(g => new AudienceEventBreakdownDto
                {
                    EventoId = g.Key,
                    Total = g.Count(),
                    Operators = g.Count(x => x.IsOperator),
                    Live = g.Count(x => !x.IsOperator)
                })
                .OrderByDescending(x => x.Total)
                .ThenBy(x => x.EventoId)
                .ToList();

            return new AudienceLiveDto
            {
                CapturedAtUtc = DateTime.UtcNow,
                TotalConnections = total,
                LiveConnections = live,
                OperatorConnections = operators,
                SoftCapacity = capacity,
                SaturationPercent = percent,
                SaturationLevel = percent >= 85 ? "critical" : percent >= 70 ? "warning" : "ok",
                SessionPeakTotal = sessionPeak,
                SessionPeakAtUtc = sessionPeakAt,
                ByEvento = byEvent
            };
        }

        private void TouchSessionPeak()
        {
            var total = _connections.Count;
            lock (_peakLock)
            {
                if (total > _sessionPeakTotal)
                {
                    _sessionPeakTotal = total;
                    _sessionPeakAtUtc = DateTime.UtcNow;
                }
            }
        }

        internal static bool IsOperatorRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return false;

            var r = role.Trim();
            if (r.Equals("Espectador", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Espectador Live", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Live", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Publico", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Público", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Anonymous", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Viewer", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private static string? Normalize(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
