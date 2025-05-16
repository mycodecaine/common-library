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

        internal SportTypeCreatedEvent(SportTypeCreatedDomainEvent orderCreatedDomainEvent, Guid correlationId)
        {
            SportTypeId = orderCreatedDomainEvent.SportType.Id;
            CorrelationId = correlationId;
        }

        [JsonConstructor]
        private SportTypeCreatedEvent(Guid sportTypeId, Guid correlationId)
        {
            SportTypeId = sportTypeId;
            CorrelationId = correlationId;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid SportTypeId { get; }
        public Guid CorrelationId { get; private set; }
    }
}
