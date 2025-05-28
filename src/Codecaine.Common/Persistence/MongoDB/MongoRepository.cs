using Codecaine.Common.Domain;
using Codecaine.Common.Pagination.Interfaces;
using Codecaine.Common.Persistence.MongoDB.Interfaces;
using Codecaine.Common.Primitives.Maybe;

namespace Codecaine.Common.Persistence.MongoDB
{
    public class MongoRepository<TEntity> : INoSqlRepository<TEntity> where TEntity : Entity
    {
        
        protected readonly IMongoDbContext _context;

        public MongoRepository(IMongoDbContext context)
        {
          
            _context = context;
        }
        public void Delete(Guid id)
        {
            var data = _context.GetBydIdAsync<TEntity>(id).Result;
            if (data.HasValue)
            {
                Remove(data.Value);
            }
        }

        public async Task<Maybe<TEntity>> GetByIdAsync(Guid id)
        {
            return await _context.GetBydIdAsync<TEntity>(id);
        }

        public Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, ISpecification<TEntity>? specification = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity entity)
        {

            _context.Insert(entity);
        }

        public void Remove(TEntity entity)
        {
            _context.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
    }
}
