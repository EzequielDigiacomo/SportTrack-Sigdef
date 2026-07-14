using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Fase;
using System;
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
    /// No poner [AllowAnonymous] a nivel clase: anularía el Authorize de los métodos.
    /// </summary>
    public class TimingHub : Hub
    {
        private readonly IFaseService _faseService;
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.List<RaceUserPresence>> _activeRaceGroups =
            new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.List<RaceUserPresence>>();

        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.List<RaceUserPresence>> _activeEventGroups =
            new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.List<RaceUserPresence>>();

        public TimingHub(IFaseService faseService)
        {
            _faseService = faseService;
        }

        [AllowAnonymous]
        public async Task JoinRaceGroup(string faseId, string userName, string role)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"race_{faseId}");

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

            if (_activeRaceGroups.TryGetValue(faseId, out var currentList))
            {
                System.Collections.Generic.List<RaceUserPresence> copyList;
                lock (currentList)
                {
                    copyList = new System.Collections.Generic.List<RaceUserPresence>(currentList);
                }
                await Clients.Group($"race_{faseId}").SendAsync("RacePresenceUpdated", copyList);
            }
        }

        [AllowAnonymous]
        public async Task JoinEventGroup(string eventoId, string userName, string role)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"event_{eventoId}");

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

            if (_activeEventGroups.TryGetValue(eventoId, out var currentList))
            {
                System.Collections.Generic.List<RaceUserPresence> copyList;
                lock (currentList)
                {
                    copyList = new System.Collections.Generic.List<RaceUserPresence>(currentList);
                }
                await Clients.Group($"event_{eventoId}").SendAsync("EventPresenceUpdated", copyList);
            }
        }

        [AllowAnonymous]
        public async Task LeaveRaceGroup(string faseId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"race_{faseId}");

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
                await Clients.Group($"race_{faseId}").SendAsync("RacePresenceUpdated", copyList);
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
                    await Clients.Group($"race_{faseId}").SendAsync("RacePresenceUpdated", copyList);
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
                    await Clients.Group($"event_{eventoId}").SendAsync("EventPresenceUpdated", copyList);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task RequestStartRace(int faseId, DateTime startTime)
        {
            await _faseService.IniciarFaseAsync(faseId, startTime);
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task RequestResetRace(int faseId)
        {
            await _faseService.ReiniciarFaseAsync(faseId);
        }

        [AllowAnonymous]
        public DateTime GetServerTime()
        {
            return DateTime.UtcNow;
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task RecordLap(int faseId, int resultadoId, string time)
        {
            await Clients.Group($"race_{faseId}").SendAsync("LapRecorded", resultadoId, time);
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task FinishRace(int faseId)
        {
            await Clients.Group($"race_{faseId}").SendAsync("RaceFinished", faseId);
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task SendTime(string faseId, string resultadoId, string timeStr, long ms)
        {
            await Clients.Group($"race_{faseId}").SendAsync("TimeReceived", resultadoId, timeStr, ms);
            await Clients.All.SendAsync("globalTimeReceived", faseId, resultadoId, timeStr, ms);
        }

        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task UpdateResultStatus(string faseId, string resultadoId, string status)
        {
            await _faseService.UpdateResultadoStatusAsync(int.Parse(resultadoId), status);
        }

        [Authorize(Roles = "Admin,SuperAdmin,Club,soporte_tecnico")]
        public async Task RequestPaymentStatusChange(string clubNombre, string clubId)
        {
            await Clients.All.SendAsync("paymentStatusChangeRequested", new { clubNombre, clubId, motive = "solicitar cambio de estado de pago de este club" });
        }
    }
}
