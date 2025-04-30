using Codecaine.Common.Abstractions;
using Codecaine.Common.Persistence.EfCore;
using Codecaine.Common.Persistence.EfCore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Codecaine.SportService.Infrastructure.DataAccess
{
    internal class DataContext : AppDbContext, IDesignTimeDbContextFactory<DataContext>
    {
        private readonly IDateTime _dateTime;
        private readonly IMediator _mediator;

        public DataContext()
        {

        }

        public DataContext(DbContextOptions options)
             : base(options)
        {

        }
        public DataContext(DbContextOptions options, IDateTime dateTime, IMediator mediator) : base(options, dateTime, mediator)
        {
            _dateTime = dateTime;
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyUtcDateTimeConverter();

            base.OnModelCreating(modelBuilder);
        }
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=SportDb;User=sa;Password=P@ssW0rd;TrustServerCertificate=True");

            return new DataContext(optionsBuilder.Options, _dateTime, _mediator);
        }
    }
}
