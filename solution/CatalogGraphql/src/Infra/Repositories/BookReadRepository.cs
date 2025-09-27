using Application.BookQueries;
using GreenDonut.Data;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class BookReadRepository : ReadRepository, IBookReadRepository
{
    public BookReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<BookDto?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.Books.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity?.ToDto();
    }

    public async ValueTask<Page<BookDto>> With(PagingArguments pagingArguments, QueryContext<BookDto> queryContext,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Books
            .Select(e => new BookDto
            {
                Id = e.Id,
                Title = e.Title,
                AuthorId = e.AuthorId,
                DatePublished = e.DatePublished,
                Price = e.Price
            })
            .With(queryContext, DefaultOrder)
            .ToPageAsync(pagingArguments, cancellationToken);
    }

    public async Task<ILookup<Guid, BookDto>> GetBooksByAuthorIds(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        return dbContext.Books
            .Where(b => ids.Contains(b.AuthorId))
            .Select(e => new BookDto
            {
                Id = e.Id,
                Title = e.Title,
                AuthorId = e.AuthorId,
                DatePublished = e.DatePublished,
                Price = e.Price
            })
            .ToLookup(b => b.AuthorId);
    }

    private static SortDefinition<BookDto> DefaultOrder(SortDefinition<BookDto> sort)
        => sort.IfEmpty(o => o.AddDescending(t => t.Id));
}
