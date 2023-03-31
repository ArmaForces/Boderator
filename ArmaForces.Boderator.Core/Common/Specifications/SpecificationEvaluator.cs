using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ArmaForces.Boderator.Core.Common.Specifications;

public static class SpecificationEvaluator<TEntity> where TEntity : class
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query, IQuerySpecification<TEntity> specifications)
    {
        // Do not apply anything if specifications is null
        if (specifications == null)
        {
            return query;
        }

        // Modify the IQueryable
        // Apply filter conditions
        if (specifications.Criteria != null)
        {
            query = query.Where(specifications.Criteria);
        }

        // Includes
        query = specifications.Includes
            .Aggregate(query, (current, include) => current.Include(include));

        // Apply ordering
        if (specifications.OrderBy != null)
        {
            query = query.OrderBy(specifications.OrderBy);
        }
        else if (specifications.OrderByDescending != null)
        {
            query = query.OrderByDescending(specifications.OrderByDescending);
        }

        return query;
    }
}
