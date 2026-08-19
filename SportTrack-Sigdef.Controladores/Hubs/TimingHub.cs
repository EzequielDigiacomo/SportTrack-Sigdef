using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SportTrack_Sigdef.Controladores.Audience;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Fase;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Hubs
{
    public class RaceUserPresence
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }

    /// <summary>
    /// Conexión: AllowAnonymous en MapHub (Program.cs) para Live + FallbackPolicy.
    /// Join/GetServerTime: [AllowAnonymous]. Escrituras: [Authorize] por método.
    /// Broadcasts de carrera van a event_{id} + operators (no Clients.All).
    /// </summary>
    public class TimingHub : Hub
    {
        private readonly IFaseService _faseService;
        private readonly IAudiencePresenceTracker _audienceTracker;
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.List<RaceUserPresence>> _activeRaceGroups =
            new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.List<RaceUserPresence>>();

        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.List<RaceUserPresence>> _activeEventGroups =
            new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.List<RaceUserPresence>>();

        public TimingHub(IFaseService faseService, IAudiencePresenceTracker audienceTracker)
        {
            _faseService = faseService;
            _audienceTracker = audienceTracker;
        }

        [AllowAnonymous]
        public async Task JoinRaceGroup(string faseId, string userName, string role)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, TimingGroups.Race(faseId));

            var presence = new RaceUserPresence
            {
                ConnectionId = Context.ConnectionId,
                UserName = userName,
                Role = role
            };

            _activeRaceGroups.AddOrUpdate(faseId,
                new System.Collections.Generic.List<RaceUserPresence> { presence },
                (key, oldValue) =>
                {
                    lock (oldValue)
                    {
                        oldValue.RemoveAll(x => x.ConnectionId == Context.ConnectionId || (x.UserName == userName && x.Role == role));
                        oldValue.Add(presence);
                    }
                    return oldValue;
                });

            _audienceTracker.Upsert(Context.ConnectionId, eventoId: null, faseId: faseId, userName: userName, role: role);

            if (_activeRaceGroups.TryGetValue(faseId, out var currentList))
            {
                System.Collections.Generic.List<RaceUserPresence> copyList;
                lock (currentList)
                {
                    copyList = new System.Collections.Generic.List<RaceUserPresence>(currentList);
                }
                await Clients.Group(TimingGroups.Race(faseId)).SendAsync("RacePresenceUpdated", copyList);
            }
        }

        [AllowAnonymous]
        public async Task JoinEventGroup(string eventoId, string userName, string role)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, TimingGroups.Event(eventoId));

            var presence = new RaceUserPresence
            {
                ConnectionId = Context.ConnectionId,
                UserName = userName,
                Role = role
            };

            _activeEventGroups.AddOrUpdate(eventoId,
                new System.Collections.Generic.List<RaceUserPresence> { presence },
                (key, oldValue) =>
                {
                    lock (oldValue)
                    {
                        oldValue.RemoveAll(x => x.ConnectionId == Context.ConnectionId || (x.UserName == userName && x.Role == role));
                        oldValue.Add(presence);
                    }
                    return oldValue;
                });

            _audienceTracker.Upsert(Context.ConnectionId, eventoId: eventoId, faseId: null, userName: userName, role: role);

            if (_activeEventGroups.TryGetValue(eventoId, out var currentList))
            {
                System.Collections.Generic.List<RaceUserPresence> copyList;
                lock (currentList)
                {
                    copyList = new System.Collections.Generic.List<RaceUserPresence>(currentList);
                }
                await Clients.Group(TimingGroups.Event(eventoId)).SendAsync("EventPresenceUpdated", copyList);
            }
        }

        /// <summary>
        /// Grupo de operadores (admin/jueces) para notificaciones sin Clients.All.
        /// </summary>
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators + ",Club")]
        public async Task JoinOperatorsGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, TimingGroups.Operators);
            _audienceTracker.MarkOperator(Context.ConnectionId);
        }

        /// <summary>Notificaciones personales (mensajes internos).</summary>
        [Authorize(Roles = "SuperAdmin,Admin,Club,JuezControl,Largador,Cronometrista,ControlTecnico,soporte_tecnico")]
        public async Task JoinUserNotificationsGroup()
        {
            var username = Context.User?.FindFirstValue(ClaimTypes.Name)
                ?? Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(username)) return;

            await Groups.AddToGroupAsync(Context.ConnectionId, TimingGroups.User(username));
        }

        /// <summary>Notificaciones de novedades para clubes/admins de una federación.</summary>
        [Authorize(Roles = "SuperAdmin,Admin,Club,soporte_tecnico")]
        public async Task JoinFederationNotificationsGroup(int federacionId)
        {
            if (federacionId <= 0) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, TimingGroups.Federation(federacionId));
        }

        [AllowAnonymous]
        public async Task LeaveRaceGroup(string faseId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, TimingGroups.Race(faseId));

            if (_activeRaceGroups.TryGetValue(faseId, out var currentList))
            {
                lock (currentList)
                {
                    currentList.RemoveAll(x => x.ConnectionId == Context.ConnectionId);
                }
                System.Collections.Generic.List<RaceUserPresence> copyList;
                lock (currentList)
                {
                    copyList = new System.Collections.Generic.List<RaceUserPresence>(currentList);
                }
                await Clients.Group(TimingGroups.Race(faseId)).SendAsync("RacePresenceUpdated", copyList);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var entry in _activeRaceGroups)
            {
                var faseId = entry.Key;
                var currentList = entry.Value;
                bool removed = false;
                lock (currentList)
                {
                    int before = currentList.Count;
                    currentList.RemoveAll(x => x.ConnectionId == Context.ConnectionId);
                    removed = currentList.Count < before;
                }
                if (removed)
                {
                    System.Collections.Generic.List<RaceUserPresence> copyList;
                    lock (currentList)
                    {
                        copyList = new System.Collections.Generic.List<RaceUserPresence>(currentList);
                    }
                    await Clients.Group(TimingGroups.Race(faseId)).SendAsync("RacePresenceUpdated", copyList);
                }
            }

            foreach (var entry in _activeEventGroups)
            {
                var eventoId = entry.Key;
                var currentList = entry.Value;
                bool removed = false;
                lock (currentList)
                {
                    int before = currentList.Count;
                    currentList.RemoveAll(x => x.ConnectionId == Context.ConnectionId);
                    removed = currentList.Count < before;
                }
                if (removed)
                {
                    System.Collections.Generic.List<RaceUserPresence> copyList;
                    lock (currentList)
                    {
                        copyList = new System.Collections.Generic.List<RaceUserPresence>(currentList);
                    }
                    await Clients.Group(TimingGroups.Event(eventoId)).SendAsync("EventPresenceUpdated", copyList);
                }
            }

            _audienceTracker.Remove(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task RequestStartRace(int faseId, DateTime startTime)
        {
            await _faseService.IniciarFaseAsync(faseId, startTime);
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task RequestResetRace(int faseId, string motivo, string? categoria = null)
        {
            var reason = string.IsNullOrWhiteSpace(motivo)
                ? "Partida en falso confirmada por largador"
                : motivo.Trim();
            var cat = string.IsNullOrWhiteSpace(categoria) ? "mala_largada" : categoria.Trim();
            await _faseService.ReiniciarFaseAsync(faseId, reason, cat);
        }

        [AllowAnonymous]
        public DateTime GetServerTime()
        {
            return DateTime.UtcNow;
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task RecordLap(int faseId, int resultadoId, string time)
        {
            await Clients.Group(TimingGroups.Race(faseId)).SendAsync("LapRecorded", resultadoId, time);
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task FinishRace(int faseId)
        {
            await Clients.Group(TimingGroups.Race(faseId)).SendAsync("RaceFinished", faseId);
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task SendTime(string faseId, string resultadoId, string timeStr, long ms)
        {
            await Clients.Group(TimingGroups.Race(faseId)).SendAsync("TimeReceived", resultadoId, timeStr, ms);

            if (int.TryParse(faseId, out var faseIdInt))
            {
                var eventoId = await _faseService.GetEventoIdByFaseIdAsync(faseIdInt);
                if (eventoId.HasValue)
                {
                    await Clients.Group(TimingGroups.Event(eventoId.Value))
                        .SendAsync("globalTimeReceived", faseId, resultadoId, timeStr, ms);
                }
            }
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task UpdateResultStatus(string faseId, string resultadoId, string status)
        {
            await _faseService.UpdateResultadoStatusAsync(int.Parse(resultadoId), status);
        }

        [Authorize(Roles = "Admin,SuperAdmin,Club,soporte_tecnico")]
        public async Task RequestPaymentStatusChange(string clubNombre, string clubId)
        {
            await Clients.Group(TimingGroups.Operators).SendAsync(
                "paymentStatusChangeRequested",
                new { clubNombre, clubId, motive = "solicitar cambio de estado de pago de este club" });
        }
    }
}
