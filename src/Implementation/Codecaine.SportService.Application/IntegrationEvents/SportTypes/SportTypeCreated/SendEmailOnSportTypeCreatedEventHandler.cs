using Codecaine.Common.CQRS.Events;
using Codecaine.SportService.Application.UseCases.SportTypes.Events.SportTypeCreated;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation($"SendEmailOnSportTypeCreatedEventHandler SportTypeId: {notification.SportTypeId}");
            return Task.CompletedTask;
        }
    }
}
