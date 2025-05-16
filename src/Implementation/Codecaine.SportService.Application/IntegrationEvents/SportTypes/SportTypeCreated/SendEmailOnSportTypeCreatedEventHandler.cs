using Codecaine.Common.CQRS.Events;
using Codecaine.SportService.Application.UseCases.SportTypes.Events.SportTypeCreated;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Codecaine.SportService.Application.IntegrationEvents.SportTypes.SportTypeCreated
{
    internal class SendEmailOnSportTypeCreatedEventHandler : IIntegrationEventHandler<SportTypeCreatedEvent>
    {
        private readonly ILogger<SendEmailOnSportTypeCreatedEventHandler> _logger;

        public SendEmailOnSportTypeCreatedEventHandler(ILogger<SendEmailOnSportTypeCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SportTypeCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SendEmailOnSportTypeCreatedEventHandler SportTypeId: {SportTypeId}, {CorrelationId}", notification.SportTypeId, notification.CorrelationId);
          
            return Task.CompletedTask;
        }
    }
}
