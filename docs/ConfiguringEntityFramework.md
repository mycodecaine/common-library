# Configuring Entity Framework with a Common Library

This guide explains how to set up Entity Framework Core in your infrastructure project using a shared persistence base class.

---

## 1. Install Required NuGet Packages

Install the following packages in your **Infrastructure** project:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.SqlServer      # If using SQL Server
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL        # If using PostgreSQL
```

---

## 2. Create the `DataContext` Class

Create a `DataContext` class under `[YourService].Infrastructure.DataAccess`.

- Inherit from `AppDbContext` provided by `Codecaine.Common.Persistence.EfCore`.
- Implement the `IDesignTimeDbContextFactory<DataContext>` interface for migrations.

### Sample `DataContext` Implementation

```csharp
namespace Codecaine.SportService.Infrastructure.DataAccess
{
    internal class DataContext : AppDbContext, IDesignTimeDbContextFactory<DataContext>
    {
        private readonly IDateTime _dateTime;
        private readonly IMediator _mediator;

        public DataContext() {}

        public DataContext(DbContextOptions options)
            : base(options) {}

        public DataContext(DbContextOptions options, IDateTime dateTime, IMediator mediator)
            : base(options, dateTime, mediator)
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
```

---

## 3. Add Entity Configurations

Create a `Configurations` folder inside `DataAccess`. Add one configuration class per entity using `IEntityTypeConfiguration<T>`.

### Sample Entity Configuration

```csharp
namespace Codecaine.SportService.Infrastructure.DataAccess.Configurations
{
    public class SportVariantConfiguration : IEntityTypeConfiguration<SportVariant>
    {
        public void Configure(EntityTypeBuilder<SportVariant> builder)
        {
            builder.ToTable("SportVariants");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(300);

            builder.Property(x => x.IsOlympic)
                .IsRequired();

            // Audit properties
            builder.Property(st => st.CreatedOnUtc).IsRequired();
            builder.Property(st => st.ModifiedOnUtc);
            builder.Property(st => st.CreatedBy);
            builder.Property(st => st.ModifiedBy);

            // Soft delete
            builder.Property(st => st.DeletedOnUtc);
            builder.Property(st => st.Deleted).HasDefaultValue(false);

            builder.Ignore(s => s.DomainEvents);

            builder.HasOne(x => x.SportType);

            // SportRule (Value Object)
            builder.OwnsOne(x => x.Rules, rule =>
            {
                rule.Property(r => r.ScoringSystem)
                    .IsRequired()
                    .HasConversion<string>();

                rule.Property(r => r.PlayerCount).IsRequired();
                rule.Property(r => r.Duration);
                rule.Property(r => r.MaxScore);
            });
        }
    }
}
```

---

## 4. Generate Migrations

From the terminal, navigate to the **Infrastructure** project directory and run:

```bash
dotnet ef migrations add "YourMigrationName"
```

## 5. Update Database

From the terminal run

````bash
dotnet ef database update
````