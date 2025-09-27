using GreenDonut.Data;

namespace Application.BookQueries;

public interface IBookReadRepository
{
    public Task<BookDto?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

    public ValueTask<Page<BookDto>> With(PagingArguments pagingArguments, QueryContext<BookDto> queryContext,
        CancellationToken cancellationToken = default);
    
    public Task<ILookup<Guid, BookDto>> GetBooksByAuthorIds(IReadOnlyList<Guid> ids, CancellationToken cancellationToken);
}