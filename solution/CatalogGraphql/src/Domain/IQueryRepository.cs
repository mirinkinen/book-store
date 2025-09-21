using Common.Domain;

namespace Domain;

public interface IQueryRepository<TEntity> where TEntity : Entity
{
    public IQueryable<TEntity> GetQuery();
}