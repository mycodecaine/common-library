using Codecaine.Common.Persistence.EfCore;
using Codecaine.Common.Persistence.EfCore.Interfaces;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Codecaine.SportService.Infrastructure.DataAccess.Specifications.SportTypes;
using Codecaine.SportService.Infrastructure.DataAccess.Specifications.SportVariants;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;

namespace Codecaine.SportService.Infrastructure.DataAccess.Repositories
{
    internal sealed class SportVariantRepository : Repository<SportVariant>, ISportVariantRepository
    {
        public SportVariantRepository(IDbContext context) : base(context)
        {
        }

        public async override Task<Maybe<SportVariant>> GetByIdAsync(Guid id)
        {
            var order = await DbContext.Set<SportVariant>().Include(a => a.SportType).Include(x => x.PlayerPositions).Include(x => x.PopularInCountries).FirstOrDefaultAsync(x => x.Id == id);
            return order;
        }

        public Task<bool> IsDuplicateNameAsync(Guid id, Guid sportTypeId, string name)
        {
            return AnyAsync(new SportVariantDuplicateNameWithDifferentIdSpecification(name, id, sportTypeId));
        }

        public Task<bool> IsNameExistAsync(Guid sportTypeId, string name)
        {
            return AnyAsync(new SportVariantWithNameSpecification(name, sportTypeId));
        }       
        public async Task<(IEnumerable<SportVariant> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? name, string? description, Guid? sportTypeId, string imageUrl, bool? isOlympic, string? sortBy, bool isDescending)
        {
            var specification = new SportVariantFilteringSpecification(pageNumber,pageSize, name, description, sportTypeId, imageUrl, isOlympic, sortBy, isDescending);
            return await GetPagedAsync(specification);
        }

    }
}
