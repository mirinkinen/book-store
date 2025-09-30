using GreenDonut.Data;

namespace Application.AuthorQueries;

public interface IAuthorReadRepository
{
    public Task<AuthorNode?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

    public ValueTask<Page<AuthorNode>> With(PagingArguments pagingArguments, QueryContext<AuthorNode> queryContext,
        CancellationToken cancellationToken = default);
}