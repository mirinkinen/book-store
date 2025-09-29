using Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace API.AuthorOperations;

[QueryType]
public static partial class AuthorQueries
{
    private static readonly ActivitySource _activity = new(nameof(AuthorQueries));
    
    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<Author>> GetAuthorEntities(
        PagingArguments pagingArguments,
        QueryContext<Author> queryContext,
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        using var activity = _activity.StartActivity();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var page = await dbContext.Authors
            .Include(a => a.Books)
            .With(queryContext, sort => sort.IfEmpty(o => o.AddDescending(t => t.Id)))
            .ToPageAsync(pagingArguments, cancellationToken);
        
        return new PageConnection<Author>(page);
    }
}