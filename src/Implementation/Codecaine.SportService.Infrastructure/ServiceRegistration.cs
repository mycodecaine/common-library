using Codecaine.Common.Abstractions;
using Codecaine.Common.Date;
using Codecaine.Common.EventConsumer;
using Codecaine.Common.Messaging;
using Codecaine.Common.Messaging.MassTransit;
using Codecaine.Common.Persistence;
using Codecaine.Common.Persistence.EfCore.Interfaces;
using Codecaine.SportService.Domain.Repositories;
using Codecaine.SportService.Infrastructure.DataAccess;
using Codecaine.SportService.Infrastructure.DataAccess.Repositories;
using Codecaine.SportService.Infrastructure.Messaging;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Codecaine.SportService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString__DataBase") ?? "";
            
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<DataContext>());

            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<DataContext>());

            // Repositories

            services.AddScoped<ISportTypeRepository, SportTypeRepository>();

            services.AddScoped<ISportVariantRepository, SportVariantRepository>();

            services.AddTransient<IDateTime, MachineDateTime>();

            // Event consumer
            services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

            // MassTransit Publisher
            services.AddScoped<IMessagePublisher, MessagePublisher>();


            services.AddMassTransitRabbitMq();

            return services;
        }

        public static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection services)
        {
            string host = Environment.GetEnvironmentVariable("RabbitMq__Host") ?? "";
            string userName = Environment.GetEnvironmentVariable("RabbitMq__UserName") ?? "";
            string password = Environment.GetEnvironmentVariable("RabbitMq__Password") ?? "";
            string defaultQueueName = Environment.GetEnvironmentVariable("RabbitMq__DefaultQueueName") ?? "";


            services.AddMassTransit(x =>
            {
                x.AddConsumer<CodecaineMessageConsumer>(); // Register Consumer

                // Add multiple consumers if needed

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, "/", h =>
                    {
                        h.Username(userName);
                        h.Password(password);
                    });

                    cfg.UseRawJsonSerializer();

                    cfg.ReceiveEndpoint(defaultQueueName, e =>
                    {
                        e.ConfigureConsumer<CodecaineMessageConsumer>(context);
                    });

                    // Configure other consumers here

                    cfg.ConfigureEndpoints(context);
                });


            });

            return services;
        }
    }
}
