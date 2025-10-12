using Domain;
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
    /// <summary>
    /// Maps a review to a review node.
    /// </summary>
    /// <remarks>Use when expression is required, for example in EF Core queries.</remarks>
    public static Expression<Func<Review, ReviewNode>> ToNode()
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