using Application.ReviewQueries;
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
        return ReviewExtensions.ProjectToNode();
    }

    protected override Func<SortDefinition<ReviewNode>, SortDefinition<ReviewNode>> GetDefaultOrder()
    {
        return sort => sort.IfEmpty(o => o.AddDescending(t => t.Id));
    }
}