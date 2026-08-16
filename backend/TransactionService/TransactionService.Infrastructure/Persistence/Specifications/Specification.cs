using TransactionService.Domain.Primitives;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace TransactionService.Infrastructure.Persistence.Specifications;

public abstract class Specification<TEntity, TId>(Expression<Func<TEntity, bool>>? criteria)
    where TEntity : Entity<TId>
{
    public Expression<Func<TEntity, bool>>? Criteria { get; private set; } = criteria;
    public List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeExpressions { get; } = new();
    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }
    public string? OrderByAsStringExpression { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) =>
        IncludeExpressions.Add(q => q.Include(includeExpression));

    protected void AddInclude<TPreviousProperty>(Expression<Func<TEntity, TPreviousProperty>> includeExpression, Expression<Func<TPreviousProperty, object>> thenIncludeExpression) =>
        IncludeExpressions.Add(q => q.Include(includeExpression).ThenInclude(thenIncludeExpression));

    protected void AddInclude<TPreviousProperty>(Expression<Func<TEntity, IEnumerable<TPreviousProperty>>> includeExpression, Expression<Func<TPreviousProperty, object>> thenIncludeExpression) =>
        IncludeExpressions.Add(q => q.Include(includeExpression).ThenInclude(thenIncludeExpression));

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) =>
        OrderByExpression = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression) =>
        OrderByDescendingExpression = orderByDescendingExpression;

    protected void AddOrderByAsString(string orderByAsStringExpression) =>
        OrderByAsStringExpression = orderByAsStringExpression;

    protected void AddCriteria(Expression<Func<TEntity, bool>> securityFilter)
    {
        if (Criteria == null)
            Criteria = securityFilter;
        else
        {
            var parameter = Criteria.Parameters[0];
            var combinedBody = Expression.AndAlso(Criteria.Body, Expression.Invoke(securityFilter, parameter));
            Criteria = Expression.Lambda<Func<TEntity, bool>>(combinedBody, parameter);
        }
    }
}