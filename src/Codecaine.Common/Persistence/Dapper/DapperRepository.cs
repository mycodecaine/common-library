using Codecaine.Common.Domain;
using Codecaine.Common.Pagination.Interfaces;
using Codecaine.Common.Persistence.Dapper.Interfaces;
using Codecaine.Common.Primitives.Maybe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Persistence.Dapper
{
    public class DapperRepository<TEntity> : IDapperRepository<TEntity> where TEntity : Entity
    {
        protected IDapperDbContext Context { get; }

        public DapperRepository(IDapperDbContext dbContext)
        {
            Context = dbContext;
        }

        public async Task Delete(Guid id)
        {
            var data = Context.GetBydIdAsync<TEntity>(id).Result;
            if (data.HasValue)
            {
              await  Remove(data.Value);
            }
           
        }

        public Task<Maybe<TEntity>> GetByIdAsync(Guid id)
        {
             return Context.GetBydIdAsync<TEntity>(id);
        }

        public Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, ISpecification<TEntity>? specification = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(Specification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task Insert(TEntity entity)
        {
           await Context.Insert(entity);
            
        }

        public async Task Remove(TEntity entity)
        {
          await Context.Remove(entity);
           
        }

        public async Task Update(TEntity entity)
        {
          await Context.Update(entity);
          
        }
    }
}
