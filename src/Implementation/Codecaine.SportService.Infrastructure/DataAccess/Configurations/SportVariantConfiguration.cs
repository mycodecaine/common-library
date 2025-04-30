using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            builder.Property(st => st.CreatedOnUtc)
                .IsRequired();

            builder.Property(st => st.ModifiedOnUtc);

            builder.Property(st => st.CreatedBy);

            builder.Property(st => st.ModifiedBy);

            // Soft delete
            builder.Property(st => st.DeletedOnUtc);
            builder.Property(st => st.Deleted)
                .HasDefaultValue(false);

            builder.Ignore(s => s.DomainEvents);

            builder.HasOne(x => x.SportType);  

            // SportRule (as value object)
            builder.OwnsOne(x => x.Rules, rule =>
            {
                rule.Property(r => r.ScoringSystem)
                    .IsRequired()
                    .HasConversion<string>(); // if enum

                rule.Property(r => r.PlayerCount)
                    .IsRequired();

                rule.Property(r => r.Duration);

                rule.Property(r => r.MaxScore);
            });

            


        }
    }
}
