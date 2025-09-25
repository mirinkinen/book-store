using GreenDonut.Data;

namespace Application.AuthorQueries.GetAuthors;

public interface IAuthorReadRepository
{
    public Task<AuthorDto?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

    public ValueTask<Page<AuthorDto>> With(PagingArguments pagingArguments, QueryContext<AuthorDto> queryContext,
        CancellationToken cancellationToken = default);
}