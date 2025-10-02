using GreenDonut.Data;

namespace Application.BookQueries;

public interface IBookReadRepository
{
    public Task<BookNode?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

    public ValueTask<Page<BookNode>> With(PagingArguments pagingArguments, QueryContext<BookNode> queryContext,
        CancellationToken cancellationToken = default);
    
    public Task<Dictionary<Guid, BookNode>> GetBooksByAuthorIds(IReadOnlyList<Guid> ids, CancellationToken cancellationToken);
}