using Codecaine.Common.Domain.Events;
using Codecaine.Common.Messaging;
using Codecaine.SportService.Domain.Events;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Events.SportVariantCreated
{


    internal class PublishIntegrationEventOnSportVariantCreatedDomainEventHandler : IDomainEventHandler<SportVariantCreatedDomainEvent>
    {
        private readonly IMessagePublisher _publisher;

        public PublishIntegrationEventOnSportVariantCreatedDomainEventHandler(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public Task Handle(SportVariantCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _publisher.PublishIntegrationEventAsync(new SportVariantCreatedEvent(notification));

            return Task.CompletedTask;
        }
    }
}
