using Codecaine.Common.CQRS.Events;
using Codecaine.Common.EventConsumer;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Codecaine.Common.Messaging.MassTransit
{
    public class MessageQueueConsumer : IConsumer<MessageWrapper>
    {
        private readonly ILogger<MessageQueueConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MessageQueueConsumer(ILogger<MessageQueueConsumer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task Consume(ConsumeContext<MessageWrapper> context)
        {
            _logger.LogInformation($"UserProfile RabbitMQ Queue trigger function processed: {context.Message.Message}");

            var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(context.Message.Message, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            using IServiceScope scope = _serviceProvider.CreateScope();

            var integrationEventConsumer = scope.ServiceProvider.GetRequiredService<IIntegrationEventConsumer>();
            if (integrationEvent != null)
            {
                integrationEventConsumer.Consume(integrationEvent);
            }

            return Task.CompletedTask;
        }
    }
}
