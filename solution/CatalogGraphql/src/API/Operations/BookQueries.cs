using Application.BookQueries.GetBook;
using Application.BookQueries.GetBooks;
using Common.Domain;
using Domain;
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

    [UsePaging(MaxPageSize = 10)]
    [UseProjection]
    [UseFiltering]
    public async Task<IQueryable<Book>> GetBooks(ISender sender)
    {
        return await sender.Send(new GetBooksQuery());
    }
}