using Codecaine.Common.Domain.Events;
using Codecaine.Common.Messaging;
using Codecaine.SportService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Events.SportTypeCreated
{
   

    internal class PublishIntegrationEventOnSportTypeCreatedDomainEventHandler : IDomainEventHandler<SportTypeCreatedDomainEvent>
    {
        private readonly IMessagePublisher _publisher;

        public PublishIntegrationEventOnSportTypeCreatedDomainEventHandler(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public Task Handle(SportTypeCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _publisher.PublishIntegrationEventAsync(new SportTypeCreatedEvent(notification));

            return Task.CompletedTask;
        }
    }
}
