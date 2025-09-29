using Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace API.BookOperations;

[QueryType]
public static partial class BookQueries
{
    private static readonly ActivitySource _activity = new(nameof(BookQueries));

    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<Book>> GetBookEntities(
        PagingArguments pagingArguments,
        QueryContext<Book> queryContext,
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        using var activity = _activity.StartActivity();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var page = await dbContext.Books
            .With(queryContext, sort => sort.IfEmpty(s => s.AddDescending(b => b.Id)))
            .ToPageAsync(pagingArguments, cancellationToken);
        
        return new PageConnection<Book>(page);
    }
}