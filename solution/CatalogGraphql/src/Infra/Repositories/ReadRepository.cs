using Common.Domain;
using Domain;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : Entity
{
    private readonly IDbContextFactory<CatalogDbContext> _dbContextFactory;

    public ReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public IQueryable<TEntity> GetQuery()
    {
        var dbContext = _dbContextFactory.CreateDbContext();
        return dbContext.Set<TEntity>().OrderBy(e => e.Id);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken: cancellationToken);
        
        return entity;
    }
}