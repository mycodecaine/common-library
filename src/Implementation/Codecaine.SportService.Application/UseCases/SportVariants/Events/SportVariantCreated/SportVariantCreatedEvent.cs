using Codecaine.Common.CQRS.Events;
using Codecaine.SportService.Domain.Events;
using Newtonsoft.Json;
using OpenTelemetry;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Events.SportVariantCreated
{


    internal class SportVariantCreatedEvent : IIntegrationEvent
    {

        internal SportVariantCreatedEvent(SportVariantCreatedDomainEvent sportVariantCreatedDomainEvent) => SportVariantId = sportVariantCreatedDomainEvent.SportVariant.Id;

        [JsonConstructor]
        private SportVariantCreatedEvent(Guid sportVariantId) => SportVariantId = sportVariantId;

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid SportVariantId { get; }
        public Guid CorrelationId => Guid.Parse(Baggage.GetBaggage("correlation_id"));
    }
}
