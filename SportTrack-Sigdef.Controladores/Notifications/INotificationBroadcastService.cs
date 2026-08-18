namespace SportTrack_Sigdef.Controladores.Notifications
{
    public interface INotificationBroadcastService
    {
        Task NotifyNewMessageAsync(string destinatarioUsername, object payload);
        Task NotifyNewEventAsync(int federacionId, object payload);
    }
}
