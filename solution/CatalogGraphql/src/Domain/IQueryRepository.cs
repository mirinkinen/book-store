using Common.Domain;
using GreenDonut.Data;

namespace Domain;

public interface IQueryRepository<TEntity> where TEntity : Entity
{
    public IQueryable<TEntity> GetQuery();

    public ValueTask<Page<TEntity>> With(PagingArguments pagingArguments, QueryContext<TEntity> queryContext);
}