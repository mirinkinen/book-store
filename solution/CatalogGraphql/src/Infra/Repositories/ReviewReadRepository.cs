using Application.ReviewQueries;
using Domain;
using Domain.Reviews;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Repositories;

public class ReviewReadRepository : ReadRepository<Review, ReviewNode>, IReviewReadRepository
{
    public ReviewReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    protected override Expression<Func<Review, ReviewNode>> GetProjection()
    {
        return ReviewExtensions.ToNode();
    }

    protected override Func<SortDefinition<ReviewNode>, SortDefinition<ReviewNode>> GetDefaultOrder()
    {
        return sort => sort.IfEmpty(o => o.AddDescending(t => t.Id));
    }

    public async Task<ILookup<Guid, ReviewNode>> GetReviewsByBookIds(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var dictionary = await dbContext.Reviews
            .Where(r => ids.Contains(r.BookId))
            .Select(ReviewExtensions.ToNode())
            .ToDictionaryAsync(r => r.Id, cancellationToken);

        return dictionary.ToLookup(r => r.Value.BookId, r => r.Value);
    }
}