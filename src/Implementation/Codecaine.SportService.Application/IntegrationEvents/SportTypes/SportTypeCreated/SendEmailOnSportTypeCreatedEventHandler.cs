using Codecaine.Common.CQRS.Events;
using Codecaine.Common.Notifications;
using Codecaine.SportService.Application.UseCases.SportTypes.Events.SportTypeCreated;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.IntegrationEvents.SportTypes.SportTypeCreated
{
    internal class SendEmailOnSportTypeCreatedEventHandler : IIntegrationEventHandler<SportTypeCreatedEvent>
    {
        private readonly ILogger<SendEmailOnSportTypeCreatedEventHandler> _logger;
        private readonly INotificationSender _notification;

        public SendEmailOnSportTypeCreatedEventHandler(ILogger<SendEmailOnSportTypeCreatedEventHandler> logger, INotificationSender notificationSender)
        {
            _logger = logger;
            _notification = notificationSender;
        }

        public Task Handle(SportTypeCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SendEmailOnSportTypeCreatedEventHandler SportTypeId: {SportTypeId}, {CorrelationId}", notification.SportTypeId, notification.CorrelationId);

            var message = new NotificationMessage
            {
                To = "i.heemi@yahoo.com",
                Subject = "New Sport Type Created",
                Body = $"A new sport type has been created with the following details:\n\n" +                      
                       $"Sport Type ID: {notification.SportTypeId}\n" +
                       $"Correlation ID: {notification.CorrelationId}"
            };

            _notification.SendAsync(message,NotificationChannel.Email);

            return Task.CompletedTask;
        }
    }
}
