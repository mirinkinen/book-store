using Cataloging.Infra.Database;
using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cataloging.Infra;

public class QueryAuthorizerRepository<TEntity> : IQueryAuthorizerRepository<TEntity> where TEntity : Entity
{
    private readonly CatalogDbContext _dbContext;

    public QueryAuthorizerRepository(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> GetQuery()
    {
        return _dbContext.Set<TEntity>().AsNoTracking();
    }
}