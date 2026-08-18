using Microsoft.AspNetCore.SignalR;
using SportTrack_Sigdef.Controladores.Hubs;

namespace SportTrack_Sigdef.Controladores.Notifications
{
    public class NotificationBroadcastService : INotificationBroadcastService
    {
        private readonly IHubContext<TimingHub> _hubContext;

        public NotificationBroadcastService(IHubContext<TimingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task NotifyNewMessageAsync(string destinatarioUsername, object payload)
        {
            if (string.IsNullOrWhiteSpace(destinatarioUsername)) return Task.CompletedTask;

            return _hubContext.Clients
                .Group(TimingGroups.User(destinatarioUsername))
                .SendAsync("newMessageReceived", payload);
        }

        public Task NotifyNewEventAsync(int federacionId, object payload)
        {
            if (federacionId <= 0) return Task.CompletedTask;

            return _hubContext.Clients
                .Group(TimingGroups.Federation(federacionId))
                .SendAsync("newEventCreated", payload);
        }
    }
}
