using Codecaine.Common.CQRS.Events;
using Codecaine.Common.Notifications;
using Codecaine.SportService.Application.UseCases.SportTypes.Events.SportTypeCreated;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.IntegrationEvents.SportTypes.SportTypeCreated
{
  
    internal class SendWhatsappOnSportTypeCreatedEventHandler : IIntegrationEventHandler<SportTypeCreatedEvent>
    {
        private readonly ILogger<SendWhatsappOnSportTypeCreatedEventHandler> _logger;
        private readonly INotificationSender _notification;

        public SendWhatsappOnSportTypeCreatedEventHandler(ILogger<SendWhatsappOnSportTypeCreatedEventHandler> logger, INotificationSender notificationSender)
        {
            _logger = logger;
            _notification = notificationSender;
        }

        public Task Handle(SportTypeCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SendWhatsappOnSportTypeCreatedEventHandler SportTypeId: {SportTypeId}, {CorrelationId}", notification.SportTypeId, notification.CorrelationId);

            var message = new NotificationMessage
            {
                To = "60126466571",
               
                Body = $"A new sport type has been created with the following details" 
                      
            };

            _notification.SendAsync(message, NotificationChannel.Whatsapp);

            return Task.CompletedTask;
        }
    }
}
