using Common.Domain;

namespace Domain;

public interface IReadRepository<TEntity> where TEntity : Entity
{
    public IQueryable<TEntity> GetQuery();

    public Task<TEntity?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);
}