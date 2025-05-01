
using Codecaine.Common.Abstractions;
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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Add Application
            builder.Services.AddApplication();
            // Add Infrastructure
            builder.Services.AddInfrastructure();
            // Version
            builder.Services.AddApiVersioning();


            // Temporary Solution before implementing Authentication with keycloak
            builder.Services.AddScoped<IRequestContext,RequestContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
