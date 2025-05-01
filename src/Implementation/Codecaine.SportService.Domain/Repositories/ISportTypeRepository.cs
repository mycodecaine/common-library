using Codecaine.Common.Persistence;
using Codecaine.SportService.Domain.Entities;

namespace Codecaine.SportService.Domain.Repositories
{
    public interface ISportTypeRepository : IRepository<SportType>
    {
        Task<bool> IsNameExist(string name);
    }
}
