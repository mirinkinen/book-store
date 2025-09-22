using Application.BookQueries.GetBookById;
using Application.BookQueries.GetBooks;
using Common.Domain;
using Domain;
using GreenDonut.Data;
using MediatR;

namespace API.Operations;

[QueryType]
public class BookQueries
{
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public async Task<Book> GetBookById(Guid id, ISender sender)
    {
        return await sender.Send(new GetBookByIdQuery(id));
    }

    [UsePaging(MaxPageSize = 10, DefaultPageSize = 10, IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<Book>> GetBooks(QueryContext<Book> queryContext, ISender sender)
    {
        return await sender.Send(new GetBooksQuery(queryContext));
    }
}