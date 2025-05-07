using Codecaine.SportService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Infrastructure.DataAccess.Configurations
{
    internal class PopularInCountryConfiguration : IEntityTypeConfiguration<PopularInCountry>
    {
        public void Configure(EntityTypeBuilder<PopularInCountry> builder)
        {
            builder.Property<Guid>("SportVariantId");

            builder.Property(r => r.CountryCode)
                    .IsRequired()
                    .HasConversion<string>(); // if enum
        }
    }
}
