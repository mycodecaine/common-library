using Codecaine.Common.Abstractions;
using Codecaine.Common.Date;
using Codecaine.Common.Persistence;
using Codecaine.Common.Persistence.EfCore.Interfaces;
using Codecaine.SportService.Domain.Repositories;
using Codecaine.SportService.Infrastructure.DataAccess;
using Codecaine.SportService.Infrastructure.DataAccess.Repositories;
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

            return services;
        }
    }
}
