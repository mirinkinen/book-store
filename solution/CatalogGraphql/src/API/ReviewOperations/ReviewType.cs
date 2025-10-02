using Application.BookQueries;
using Application.ReviewQueries;
using Infra.DataLoaders;

namespace API.ReviewOperations;

[ObjectType<ReviewNode>]
public static partial class ReviewType
{
    public static async Task<BookNode?> GetBookAsync([Parent] ReviewNode review, IBookByReviewIdDataLoader dataLoader)
    {
        var book = await dataLoader.LoadAsync(review.Id);
        return book;
    }
}