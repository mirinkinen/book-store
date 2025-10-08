using Application.BookQueries;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class BookReadRepository : ReadRepository, IBookReadRepository
{
    public BookReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<BookNode?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.Books.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity?.ToDto();
    }

    public async ValueTask<Page<BookNode>> With(PagingArguments pagingArguments, QueryContext<BookNode> queryContext,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Books
            .Select(b => new BookNode
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                AuthorId = b.AuthorId,
                DatePublished = b.DatePublished
            })
            .With(queryContext, DefaultOrder)
            .ToPageAsync(pagingArguments, cancellationToken);
    }

    public async Task<Dictionary<Guid, BookNode>> GetBooksByAuthorIds(IReadOnlyList<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Books
            .Where(b => ids.Contains(b.AuthorId))
            .Select(b => new BookNode
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                AuthorId = b.AuthorId,
                DatePublished = b.DatePublished
            })
            .ToDictionaryAsync(b => b.AuthorId, cancellationToken);
    }

    private static SortDefinition<BookNode> DefaultOrder(SortDefinition<BookNode> sort)
        => sort.IfEmpty(o => o.AddDescending(t => t.Id));
}