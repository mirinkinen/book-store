using Common.Domain;
using MediatR;

namespace Application.ReviewQueries.GetReviewById;

public record GetReviewByIdQuery(Guid Id) : IRequest<ReviewNode>;

public class GetReviewByIdHandler : IRequestHandler<GetReviewByIdQuery, ReviewNode>
{
    private readonly IReviewReadRepository _reviewReadRepository;

    public GetReviewByIdHandler(IReviewReadRepository reviewReadRepository)
    {
        _reviewReadRepository = reviewReadRepository;
    }

    public async Task<ReviewNode> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await _reviewReadRepository.FirstOrDefaultAsync(request.Id, cancellationToken);

        if (review is null)
        {
            throw new EntityNotFoundException("Review not found", "review-not-found");
        }

        return review;
    }
}