using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.ValueObjects;

namespace Codecaine.SportService.Infrastructure.DataAccess.Configurations
{
    internal class SportTypeConfiguration : IEntityTypeConfiguration<SportType>
    {
        public void Configure(EntityTypeBuilder<SportType> builder)
        {
            builder.ToTable("SportTypes");

            // Primary Key
            builder.HasKey(st => st.Id);

            // Basic properties
            builder.Property(st => st.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(st => st.Description)
                .HasMaxLength(500);

            builder.Property(st => st.ImageUrl)
                .HasMaxLength(2048);            
                

            builder.Ignore(s => s.DomainEvents);

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
        }
    }

}
