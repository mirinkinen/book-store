using Common.Domain;
using Domain;
using GreenDonut.Data;
using Infra.Data;

namespace Infra.Repositories;

public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : Entity
{
    private readonly CatalogDbContext _dbContext;

    public QueryRepository(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> GetQuery()
    {
        return _dbContext.Set<TEntity>();
    }
    
    public IQueryable<TEntity> With(QueryContext<TEntity> queryContext)
    {
        return _dbContext.Set<TEntity>().With(queryContext);
    }
}