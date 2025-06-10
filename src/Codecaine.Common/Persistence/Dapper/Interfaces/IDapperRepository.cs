using Codecaine.Common.Domain;

namespace Codecaine.Common.Persistence.Dapper.Interfaces
{
    public interface IDapperRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
    }
}
