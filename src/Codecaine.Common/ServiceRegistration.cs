using Codecaine.Common.Abstractions;
using Codecaine.Common.Authentication;
using Codecaine.Common.Authentication.Providers.Services;
using Codecaine.Common.Date;
using Codecaine.Common.EventConsumer;
using Codecaine.Common.Extensions;
using Codecaine.Common.HttpServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Codecaine.Common
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCommonLibrary(this IServiceCollection services)
        {
            services.AddCompression();
            services.AddHttpClientWithPolicy();

            services.AddTransient<IDateTime, MachineDateTime>();

            // Event consumer
            services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

            // HttpService
            services.AddScoped<IHttpService, HttpService>();

            // Authentication - Jwt Service
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }

        public static IApplicationBuilder UseCommonLibraryBuilder(this IApplicationBuilder app)
        {
            app.UseCompression();

            return app;
        }
    }
}
