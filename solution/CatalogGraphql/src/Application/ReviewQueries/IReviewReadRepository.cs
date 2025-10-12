using Application.Common;
using Domain;

namespace Application.ReviewQueries;

public interface IReviewReadRepository : IReadRepository<Review, ReviewNode>
{
}