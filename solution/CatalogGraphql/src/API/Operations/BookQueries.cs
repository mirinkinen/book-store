using Application.BookQueries;
using Application.BookQueries.GetBookById;
using Application.BookQueries.GetBooks;
using Common.Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using MediatR;
using System.Diagnostics;

namespace API.Operations;

[QueryType]
public static partial class BookQueries
{
    private static readonly ActivitySource _activity = new(nameof(BookQueries));

    [NodeResolver]
    [Error<EntityNotFoundException>]
    public static async Task<BookDto> GetBookById(Guid id, ISender sender)
    {
        return await sender.Send(new GetBookByIdQuery(id));
    }

    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<BookDto>> GetBooks(
        PagingArguments pagingArguments,
        QueryContext<BookDto> queryContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        using var activity = _activity.StartActivity();

        var page = await sender.Send(new GetBooksQuery(pagingArguments, queryContext), cancellationToken);
        return new PageConnection<BookDto>(page);
    }
}