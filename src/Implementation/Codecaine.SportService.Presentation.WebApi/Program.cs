
using Codecaine.Common;
using Codecaine.Common.Abstractions;
using Codecaine.Common.AspNetCore.Middleware;
using Codecaine.Common.Correlation;
using Codecaine.Common.Telemetry;
using Codecaine.SportService.Application;
using Codecaine.SportService.Infrastructure;
using Codecaine.SportService.Presentation.WebApi.Context;
using Scalar.AspNetCore;

namespace Codecaine.SportService.Presentation.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add Telemetry Service from common library
            builder.AddTelemetryRegistration();



            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            


            // Add Application
            builder.Services.AddApplication();
            // Add Infrastructure
            builder.Services.AddInfrastructure();
            // Version
            builder.Services.AddApiVersioning();

           

            builder .Services.AddHttpContextAccessor();


            // Temporary Solution before implementing Authentication with keycloak
            builder.Services.AddScoped<IRequestContext, RequestContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // TODO : follow this https://yogeshhadiya33.medium.com/implement-scalar-in-net-api-91d284479d1d
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Title = "Codecaine Sport Service API";
                    options.Theme = ScalarTheme.Default;
                    
                });
            }



           
            app.UseHttpsRedirection();

            // Add Telemetry Service from common library
            app.UseTelemetryRegistration();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCommonLibraryBuilder();

            app.MapControllers();

            app.Run();
        }
    }

   


}
