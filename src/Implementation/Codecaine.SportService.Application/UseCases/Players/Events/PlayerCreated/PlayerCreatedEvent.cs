using Codecaine.Common.CQRS.Events;
using Codecaine.SportService.Domain.Events;
using Newtonsoft.Json;

namespace Codecaine.SportService.Application.UseCases.Players.Events.PlayerCreated
{

    internal class PlayerCreatedEvent : IIntegrationEvent
    {

        internal PlayerCreatedEvent(PlayerCreatedDomainEvent playerCreatedDomainEvent, Guid correlationId)
        {
            PlayerId = playerCreatedDomainEvent.Player.Id;
            CorrelationId = correlationId;
        }

        [JsonConstructor]
        private PlayerCreatedEvent(Guid playerId, Guid correlationId)
        {
            PlayerId = playerId;
            CorrelationId = correlationId;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid PlayerId { get; }
        public Guid CorrelationId { get; private set; }
    }
}
