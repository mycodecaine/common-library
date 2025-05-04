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
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CodecaineMessageConsumer>(); // Register Consumer

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.UseRawJsonSerializer();

                    cfg.ReceiveEndpoint("codecaine-message", e =>
                    {
                        e.ConfigureConsumer<CodecaineMessageConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });


            });

            return services;
        }
    }
}
