using Application.ReviewQueries;
using GreenDonut.Data;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ReviewReadRepository : ReadRepository, IReviewReadRepository
{
    public ReviewReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<ReviewNode?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.Reviews.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity?.ToDto();
    }

    public async ValueTask<Page<ReviewNode>> With(PagingArguments pagingArguments, QueryContext<ReviewNode> queryContext,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Reviews
            .Select(r => new ReviewNode
            {
                Id = r.Id,
                Title = r.Title,
                Body = r.Body,
                BookId = r.BookId
            })
            .With(queryContext, DefaultOrder)
            .ToPageAsync(pagingArguments, cancellationToken);
    }

    public async Task<ILookup<Guid, ReviewNode>> GetReviewsByBookIds(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        return dbContext.Reviews
            .Where(r => ids.Contains(r.BookId))
            .Select(r => new ReviewNode
            {
                Id = r.Id,
                Title = r.Title,
                Body = r.Body,
                BookId = r.BookId
            })
            .ToLookup(r => r.BookId);
    }

    private static SortDefinition<ReviewNode> DefaultOrder(SortDefinition<ReviewNode> sort)
        => sort.IfEmpty(o => o.AddDescending(t => t.Id));
}
