using GreenDonut.Data;

namespace Application.ReviewQueries;

public interface IReviewReadRepository
{
    public Task<ReviewNode?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

    public ValueTask<Page<ReviewNode>> With(PagingArguments pagingArguments, QueryContext<ReviewNode> queryContext,
        CancellationToken cancellationToken = default);

    public Task<ILookup<Guid, ReviewNode>> GetReviewsByBookIds(IReadOnlyList<Guid> ids, CancellationToken cancellationToken);
}