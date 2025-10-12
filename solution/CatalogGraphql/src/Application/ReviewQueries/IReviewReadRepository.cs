using Application.Services;
using Domain;
using Domain.Reviews;

namespace Application.ReviewQueries;

public interface IReviewReadRepository : IReadRepository<Review, ReviewNode>
{
}