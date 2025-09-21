using Common.Domain;

namespace Domain;

public interface IReadRepository<TEntity> where TEntity : Entity
{
    public Task<TEntity?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);
}