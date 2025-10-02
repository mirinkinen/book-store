using System.Runtime.InteropServices;
using Domain;

namespace Application.ReviewQueries;

public class ReviewNode
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public string Body { get; set; }

    public Guid BookId { get; set; }
}

public static class ReviewExtensions
{
    public static ReviewNode ToDto(this Review review)
    {
        return new ReviewNode
        {
            Id = review.Id,
            Title = review.Title,
            Body = review.Body,
            BookId = review.BookId
        };
    }
}
