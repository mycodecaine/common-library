namespace Codecaine.Common.Notifications
{
    public interface INotificationSender
    {
        Task SendAsync(NotificationMessage message, NotificationChannel channel);
    }
}
