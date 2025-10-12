using GreenDonut.Data;
using MediatR;

namespace Application.ReviewQueries.GetReviews;

public record GetReviewsQuery : IRequest<Page<ReviewNode>>
{
    public PagingArguments PagingArguments { get; }
    public QueryContext<ReviewNode> QueryContext { get; }

    public GetReviewsQuery(PagingArguments pagingArguments, QueryContext<ReviewNode> queryContext)
    {
        PagingArguments = pagingArguments;
        QueryContext = queryContext;
    }
}

public class GetReviewsHandler : IRequestHandler<GetReviewsQuery, Page<ReviewNode>>
{
    private readonly IReviewReadRepository _readRepository;

    public GetReviewsHandler(IReviewReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<Page<ReviewNode>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.GetPage(request.PagingArguments, request.QueryContext, cancellationToken).AsTask();
    }
}