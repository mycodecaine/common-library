using Codecaine.SportService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Infrastructure.DataAccess.Configurations
{
    public class PlayerPositionConfiguration : IEntityTypeConfiguration<PlayerPosition>
    {
        public void Configure(EntityTypeBuilder<PlayerPosition> builder)
        {
            builder.Property<Guid>("SportVariantId");
            

           
           
        }
    }
}
