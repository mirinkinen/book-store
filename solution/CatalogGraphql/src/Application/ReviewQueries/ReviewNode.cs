using Domain;
using Domain.Reviews;
using HotChocolate;
using System.Linq.Expressions;

namespace Application.ReviewQueries;

[GraphQLName("Review")]
public class ReviewNode
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public Guid BookId { get; set; }
}

public static class ReviewExtensions
{
    private static readonly Lazy<Func<Review, ReviewNode>> _compiledProjection = new(() => ProjectToNode().Compile());
    
    /// <summary>
    /// Maps a review to a review node.
    /// </summary>
    public static ReviewNode MapToDto(this Review review)
    {
        return _compiledProjection.Value(review);
    }
    
    /// <summary>
    /// Projects a review to a review node.
    /// </summary>
    public static Expression<Func<Review, ReviewNode>> ProjectToNode()
    {
        return review => new ReviewNode
        {
            Id = review.Id,
            Title = review.Title,
            Body = review.Body,
            BookId = review.BookId
        };
    }
}