using Codecaine.Common.Exceptions;
using FluentEmail.Core;

namespace Codecaine.Common.Notifications.Email
{
    public class EmailNotificationSender : INotificationChannelSender
    {
        private readonly IFluentEmail _email;

        public EmailNotificationSender(IFluentEmail email)
        {
            _email = email;
        }
        public NotificationChannel Channel => NotificationChannel.Email;

        public async Task SendAsync(NotificationMessage message)
        {
            var response = await _email
            .To(message.To)
            .Subject(message.Subject ?? "No Subject")
            .Body(message.Body, isHtml: true)
            .SendAsync();

            if (!response.Successful)
                throw new CommonLibraryException(new Primitives.Errors.Error("EmailNotificationSenderException", "Cannot Send Email"));

            
        }
    }
}
