using Codecaine.Common.Domain;
using Codecaine.Common.Pagination.Interfaces;
using Codecaine.Common.Persistence.MongoDB.Interfaces;
using Codecaine.Common.Primitives.Maybe;

namespace Codecaine.Common.Persistence.MongoDB
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        
        protected IMongoDbContext Context { get; }

        public MongoRepository(IMongoDbContext context)
        {
          
            Context = context;
        }
        public void Delete(Guid id)
        {
            var data = Context.GetBydIdAsync<TEntity>(id).Result;
            if (data.HasValue)
            {
                Remove(data.Value);
            }
        }

        public async Task<Maybe<TEntity>> GetByIdAsync(Guid id)
        {
            return await Context.GetBydIdAsync<TEntity>(id);
        }

        public Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, ISpecification<TEntity>? specification = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity entity)
        {

            Context.Insert(entity);
        }

        public void Remove(TEntity entity)
        {
            Context.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            Context.Update(entity);
        }
    }
}
