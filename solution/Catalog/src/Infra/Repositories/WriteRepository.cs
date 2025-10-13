using Common.Domain;
using Domain.Reviews;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public abstract class WriteRepository<TEntity>(CatalogDbContext dbContext) : IWriteRepository<TEntity> where TEntity : Entity
{
    protected CatalogDbContext DbContext { get; } = dbContext;

    public async Task<TEntity?> FirstOrDefaultAsync(Guid id)
    {
        return await DbContext.Set<TEntity>().FirstOrDefaultAsync(a => a.Id == id);
    }

    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var count = await DbContext.Set<TEntity>().Where(e => e.Id == id).ExecuteDeleteAsync(cancellationToken);
        return count > 0;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}