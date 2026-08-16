using TransactionService.Domain.Primitives;
using System.Linq.Dynamic.Core;

namespace TransactionService.Infrastructure.Persistence.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity, TId>(IQueryable<TEntity> inputQueryable, Specification<TEntity, TId> specification) where TEntity : Entity<TId>
    {
        var queryable = inputQueryable;
        
        if (specification.Criteria is not null)
            queryable = queryable.Where(specification.Criteria);


        queryable = specification.IncludeExpressions.Aggregate(queryable, (current, include) => include(current));
        
        if (specification.OrderByAsStringExpression is not null)
            queryable = queryable.OrderBy(specification.OrderByAsStringExpression);
        if (specification.OrderByExpression is not null)
            queryable = queryable.OrderBy(specification.OrderByExpression);
        else if (specification.OrderByDescendingExpression is not null)
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);

        return queryable;

    }
}