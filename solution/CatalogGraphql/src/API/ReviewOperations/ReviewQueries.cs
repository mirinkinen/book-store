// using Application.BookQueries;
// using Application.BookQueries.GetBookById;
// using Application.BookQueries.GetBooks;
// using Common.Domain;
// using GreenDonut.Data;
// using HotChocolate.Types.Pagination;
// using MediatR;
//
// namespace API.ReviewOperations;
//
// [QueryType]
// public static partial class ReviewQueries
// {
//     [NodeResolver]
//     [Error<EntityNotFoundException>]
//     public static async Task<ReviewNode> GetReviewById(Guid id, ISender sender)
//     {
//         return await sender.Send(new GetReviewByIdQuery(id));
//     }
//
//     [UseConnection]
//     [UseFiltering]
//     [UseSorting]
//     public static async Task<PageConnection<ReviewNode>> GetReviews(
//         PagingArguments pagingArguments,
//         QueryContext<ReviewNode> queryContext,
//         ISender sender,
//         CancellationToken cancellationToken)
//     {
//         var page = await sender.Send(new GetReviewsQuery(pagingArguments, queryContext), cancellationToken);
//         return new PageConnection<ReviewNode>(page);
//     }
// }