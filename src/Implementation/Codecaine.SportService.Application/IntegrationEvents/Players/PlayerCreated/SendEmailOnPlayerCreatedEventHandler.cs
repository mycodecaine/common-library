﻿using Codecaine.Common.CQRS.Events;
using Codecaine.SportService.Application.UseCases.Players.Events.PlayerCreated;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.IntegrationEvents.Players.PlayerCreated
{


    internal class SendEmailOnPlayerCreatedEventHandler : IIntegrationEventHandler<PlayerCreatedEvent>
    {
        private readonly ILogger<SendEmailOnPlayerCreatedEventHandler> _logger;

        public SendEmailOnPlayerCreatedEventHandler(ILogger<SendEmailOnPlayerCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(PlayerCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SendEmailOnPlayerCreatedEventHandler PlayerId: {PlayerId}, {CorrelationId}", notification.PlayerId, notification.CorrelationId);

            return Task.CompletedTask;
        }
    }
}
