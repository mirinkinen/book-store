using Common.Domain;

namespace Domain;

public interface IWriteRepository<TEntity> where TEntity : Entity
{
    public Task<TEntity?> FirstOrDefaultAsync(Guid id);

    public void Add(TEntity entity);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

