using Codecaine.Common.Abstractions;
using Codecaine.Common.Domain.Events;
using Codecaine.Common.Messaging;
using Codecaine.SportService.Domain.Events;
using OpenTelemetry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Events.SportTypeCreated
{
   

    internal class PublishIntegrationEventOnSportTypeCreatedDomainEventHandler : IDomainEventHandler<SportTypeCreatedDomainEvent>
    {
        private readonly IMessagePublisher _publisher;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public PublishIntegrationEventOnSportTypeCreatedDomainEventHandler(IMessagePublisher publisher, ICorrelationIdGenerator correlationIdGenerator)
        {
            _publisher = publisher;
            _correlationIdGenerator = correlationIdGenerator;
        }

        public Task Handle(SportTypeCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdGenerator.Get();

            _publisher.PublishIntegrationEventAsync(new SportTypeCreatedEvent(notification,correlationId));

            return Task.CompletedTask;
        }
    }
}
