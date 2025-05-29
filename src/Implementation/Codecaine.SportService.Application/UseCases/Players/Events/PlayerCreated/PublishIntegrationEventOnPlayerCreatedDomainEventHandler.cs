using Codecaine.Common.Abstractions;
using Codecaine.Common.Domain.Events;
using Codecaine.Common.Messaging;
using Codecaine.SportService.Domain.Events;

namespace Codecaine.SportService.Application.UseCases.Players.Events.PlayerCreated
{


    internal class PublishIntegrationEventOnPlayerCreatedDomainEventHandler : IDomainEventHandler<PlayerCreatedDomainEvent>
    {
        private readonly IMessagePublisher _publisher;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public PublishIntegrationEventOnPlayerCreatedDomainEventHandler(IMessagePublisher publisher, ICorrelationIdGenerator correlationIdGenerator)
        {
            _publisher = publisher;
            _correlationIdGenerator = correlationIdGenerator;
        }

        public Task Handle(PlayerCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdGenerator.Get();

            _publisher.PublishIntegrationEventAsync(new PlayerCreatedEvent(notification, correlationId));

            return Task.CompletedTask;
        }
    }
}
