using Common.Domain;
using GreenDonut.Data;

namespace Domain;

public interface IQueryRepository<TEntity> where TEntity : Entity
{
    public IQueryable<TEntity> GetQuery();

    public IQueryable<TEntity> With(QueryContext<TEntity> queryContext);
}