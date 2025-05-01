using Codecaine.Common.Persistence.EfCore;
using Codecaine.Common.Persistence.EfCore.Interfaces;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;

namespace Codecaine.SportService.Infrastructure.DataAccess.Repositories
{
    internal sealed class SportVariantRepository: Repository<SportVariant>, ISportVariantRepository
    {
        public SportVariantRepository(IDbContext context) : base(context)
        {
        }

        public async override Task<Maybe<SportVariant>> GetByIdAsync(Guid id)
        {
            var order = await DbContext.Set<SportVariant>().Include(a => a.SportType ).FirstOrDefaultAsync(x => x.Id == id);
            return order;
        }
    }
    
}
