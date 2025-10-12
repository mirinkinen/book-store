using Common.Domain;
using GreenDonut.Data;

namespace Application.Common;

public interface IReadRepository<TEntity, TNode> 
    where TEntity : Entity
    where TNode : class
{
    Task<TNode?> GetFirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);
    
    ValueTask<Page<TNode>> GetPage(PagingArguments pagingArguments, QueryContext<TNode> queryContext,
        CancellationToken cancellationToken = default);
}