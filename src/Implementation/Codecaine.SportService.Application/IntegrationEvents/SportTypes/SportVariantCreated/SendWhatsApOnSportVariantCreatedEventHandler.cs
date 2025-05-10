using Codecaine.Common.CQRS.Events;
using Codecaine.SportService.Application.UseCases.SportVariants.Events.SportVariantCreated;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.IntegrationEvents.SportTypes.SportVariantCreated
{

    internal class SendWhatsApOnSportVariantCreatedEventHandler : IIntegrationEventHandler<SportVariantCreatedEvent>
    {
        private readonly ILogger<SendWhatsApOnSportVariantCreatedEventHandler> _logger;

        public SendWhatsApOnSportVariantCreatedEventHandler(ILogger<SendWhatsApOnSportVariantCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SportVariantCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SendWhatsApOnSportVariantCreatedEventHandler SportVariantId: {SportVariantId}", notification.SportVariantId);
            return Task.CompletedTask;
        }
    }
}
