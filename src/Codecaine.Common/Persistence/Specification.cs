using Codecaine.Common.Domain;
using System.Linq.Expressions;

namespace Codecaine.Common.Persistence
{
    public abstract class Specification<TEntity>
        where TEntity : Entity
    {

        public int? Skip { get; protected set; }
        public int? Take { get; protected set; }

        /// <summary>
        /// Converts the specification to an expression predicate.
        /// </summary>
        /// <returns>The expression predicate.</returns>
        public abstract Expression<Func<TEntity, bool>> ToExpression();

        public Expression<Func<TEntity, object>>? OrderBy { get; protected set; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; protected set; }

        public string? OrderByString { get; protected set; }
        public string? OrderByDescendingString { get; protected set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; } = new();

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }



        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        public bool IsDistinct { get; protected set; }

        protected void ApplyDistinct() => IsDistinct = true;

        protected void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression) =>
            OrderBy = orderByExpression;

        protected void ApplyOrderBy(string propertyName) =>
            OrderByString = propertyName;

        protected void ApplyOrderByDescending(string propertyName) =>
            OrderByDescendingString = propertyName;

        protected void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression) =>
            OrderByDescending = orderByDescExpression;
        /// <summary>
        /// Checks if the specified entity satisfies this specification.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>True if the entity satisfies the specification, otherwise false.</returns>
        public bool IsSatisfiedBy(TEntity entity) => ToExpression().Compile()(entity);

        public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification) =>
            specification.ToExpression();
    }
}
