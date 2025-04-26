using Codecaine.Common.Domain;
using Codecaine.Common.Persistence.EfCore.Interfaces;
using Codecaine.Common.Primitives.Maybe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Persistence.EfCore
{
    /// <summary>
    /// Represents a generic repository for managing entities in a relational database context.
    /// Provides methods for CRUD operations and querying entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected Repository(IDbContext dbContext) => DbContext = dbContext;

        protected IDbContext DbContext { get; }

        /// <summary>
        /// Deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        public async void Delete(Guid id)
        {
            var entity = await DbContext.GetBydIdAsync<TEntity>(id);
            var data = entity.Value;
            Remove(data);
        }

        /// <summary>
        /// Gets the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>A maybe instance that may contain the entity with the specified identifier.</returns>
        public virtual async Task<Maybe<TEntity>> GetByIdAsync(Guid id) => await DbContext.GetBydIdAsync<TEntity>(id);

        /// <summary>
        /// Gets a queryable collection of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the query.</param>
        /// <returns>A queryable collection of entities that match the specified filter.</returns>
        public async Task<IQueryable<TEntity>> GetQueryable(QueryFilter<TEntity> filter)
        {
            var dataQ = DbContext.Set<TEntity>().AsQueryable();
            var data = await Task.Run(() => dataQ.Where(filter.Combine()));
            return data;
        }

        /// <summary>
        /// Inserts the specified entity into the database.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public void Insert(TEntity entity) => DbContext.Insert(entity);

        /// <summary>
        /// Paginates the entities that match the specified filter.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="orderBy">The property to order by.</param>
        /// <param name="isDecending">Whether to order in descending order.</param>
        /// <param name="filter">The filter to apply to the query.</param>
        /// <returns>A paginated result of entities that match the specified filter.</returns>
        public async Task<PagedResult<TEntity>> Pagination(int pageNumber, int itemsPerPage, string orderBy, bool isDecending, QueryFilter<TEntity> filter)
        {
            var data = DbContext.Set<TEntity>().AsQueryable();
            var query = await Task.Run(() => data.Where(filter.Combine()).OrderByMember(orderBy, isDecending).GetPaged(pageNumber, itemsPerPage));

            return query;
        }

        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

        /// <summary>
        /// Removes the specified entity from the database.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public void Remove(TEntity entity) => DbContext.Remove(entity);

        /// <summary>
        /// Checks if any entity meets the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>True if any entity meets the specified specification, otherwise false.</returns>
        protected async Task<bool> AnyAsync(Specification<TEntity> specification) =>
            await DbContext.Set<TEntity>().AnyAsync(specification);

        /// <summary>
        /// Gets the first entity that meets the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The maybe instance that may contain the first entity that meets the specified specification.</returns>
        protected async Task<Maybe<TEntity?>> FirstOrDefaultAsync(Specification<TEntity> specification)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(specification);
        }

        /// <summary>
        /// Gets a queryable collection of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the query.</param>
        /// <returns>A queryable collection of entities that match the specified filter.</returns>
        public Task<IQueryable<TEntity>> GetQueryableAsync(QueryFilter<TEntity> filter)
        {
            var dataQ = DbContext.Set<TEntity>().AsQueryable();
            var data = Task.Run(() => dataQ.Where(filter.Combine()));
            return data;
        }

        /// <summary>
        /// Paginates the entities that match the specified filter.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="orderBy">The property to order by.</param>
        /// <param name="isDecending">Whether to order in descending order.</param>
        /// <param name="filter">The filter to apply to the query.</param>
        /// <returns>A paginated result of entities that match the specified filter.</returns>
        public Task<PagedResult<TEntity>> PaginationAsync(int pageNumber, int itemsPerPage, string orderBy, bool isDecending, QueryFilter<TEntity> filter)
        {
            var data = DbContext.Set<TEntity>().AsQueryable();
            var query = Task.Run(() => data.Where(filter.Combine()).OrderByMember(orderBy, isDecending).GetPaged(pageNumber, itemsPerPage));

            return query;
        }
    }
}
