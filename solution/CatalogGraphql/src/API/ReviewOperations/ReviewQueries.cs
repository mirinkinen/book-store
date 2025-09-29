using Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace API.ReviewOperations;

[QueryType]
public static partial class ReviewQueries
{
    private static readonly ActivitySource _activity = new(nameof(ReviewQueries));

    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<Review>> GetReviewEntities(
        PagingArguments pagingArguments,
        QueryContext<Review> queryContext,
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        using var activity = _activity.StartActivity();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var page = await dbContext.Reviews
            .With(queryContext, sort => sort.IfEmpty(s => s.AddDescending(b => b.Id)))
            .ToPageAsync(pagingArguments, cancellationToken);
        
        return new PageConnection<Review>(page);
    }
}