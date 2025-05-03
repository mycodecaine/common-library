using Codecaine.Common.CQRS.Events;
using Codecaine.SportService.Domain.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Events.SportTypeCreated
{
    internal class SportTypeCreatedEvent : IIntegrationEvent
    {

        internal SportTypeCreatedEvent(SportTypeCreatedDomainEvent orderCreatedDomainEvent) => SportTypeId = orderCreatedDomainEvent.SportType.Id;

        [JsonConstructor]
        private SportTypeCreatedEvent(Guid sportTypeId) => SportTypeId = sportTypeId;

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid SportTypeId { get; }
        public Guid CorrelationId => Guid.NewGuid();
    }
}
