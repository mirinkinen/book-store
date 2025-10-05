using Application.BookQueries;
using Application.BookQueries.GetBookById;
using Application.BookQueries.GetBooks;
using Common.Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using MediatR;

namespace API.BookOperations;

[QueryType]
public static partial class BookQueries
{
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public static async Task<BookNode?> GetBookById(Guid id, ISender sender)
    {
        return await sender.Send(new GetBookByIdQuery(id));
    }

    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<BookNode>> GetBooks(
        PagingArguments pagingArguments,
        QueryContext<BookNode> queryContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new GetBooksQuery(pagingArguments, queryContext), cancellationToken);
        return new PageConnection<BookNode>(page);
    }
}