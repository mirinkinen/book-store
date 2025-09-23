using Application.BookQueries.GetBookById;
using Application.BookQueries.GetBooks;
using Common.Domain;
using Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using MediatR;

namespace API.Operations;

[QueryType]
public static partial class BookQueries
{
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public static async Task<Book> GetBookById(Guid id, ISender sender)
    {
        return await sender.Send(new GetBookByIdQuery(id));
    }

    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<Book>> GetBooks(
        PagingArguments pagingArguments, 
        QueryContext<Book> queryContext, 
        ISender sender, 
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new GetBooksQuery(pagingArguments, queryContext), cancellationToken);
        return new PageConnection<Book>(page);
    }
}