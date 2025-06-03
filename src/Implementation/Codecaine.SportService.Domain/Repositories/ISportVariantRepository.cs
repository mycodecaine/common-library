using Codecaine.Common.Persistence;
using Codecaine.SportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Domain.Repositories
{
    public interface ISportVariantRepository:IRepository<SportVariant>
    {
        Task<bool> IsNameExistAsync( Guid sportTypeId, string name);
        Task<bool> IsDuplicateNameAsync(Guid id, Guid sportTypeId, string name);
        Task<(IEnumerable<SportVariant> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? name, string? description,
            Guid? sportTypeId, string imageUrl, bool? isOlympic, string? sortBy, bool isDescending);
    }
}
