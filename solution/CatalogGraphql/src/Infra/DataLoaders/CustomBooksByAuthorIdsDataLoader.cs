using Application.BookQueries;
using GreenDonut;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.DataLoaders;

public class CustomBooksByAuthorIdsDataLoader : BatchDataLoader<Guid, IEnumerable<BookNode>>
{
    private readonly IDbContextFactory<CatalogDbContext> _contextFactory;

    public CustomBooksByAuthorIdsDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options,
        IDbContextFactory<CatalogDbContext> contextFactory)
        : base(batchScheduler, options)
    {
        _contextFactory = contextFactory;
    }

    protected override async Task<IReadOnlyDictionary<Guid, IEnumerable<BookNode>>> LoadBatchAsync(
        IReadOnlyList<Guid> authorIds,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var books = await dbContext.Books
            .Where(b => authorIds.Contains(b.AuthorId))
            .Select(b => new BookNode
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                AuthorId = b.AuthorId,
                DatePublished = b.DatePublished
            })
            .OrderBy(b => b.Id)
            .ToListAsync(cancellationToken);

        return books.ToLookup(b => b.AuthorId)
            .ToDictionary(g => g.Key, g => g.AsEnumerable());
    }

    public async Task<Page<BookNode>> LoadPageAsync(
        Guid authorId,
        PagingArguments pagingArgs,
        QueryContext<BookNode> query,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Books
            .Where(b => b.AuthorId == authorId)
            .Select(b => new BookNode
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                AuthorId = b.AuthorId,
                DatePublished = b.DatePublished
            })
            .With(query, sort => sort.IfEmpty(o => o.AddDescending(t => t.Id)))
            .ToPageAsync(pagingArgs, cancellationToken);
    }
}