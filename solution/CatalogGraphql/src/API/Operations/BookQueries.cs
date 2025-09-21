using Application.BookQueries.GetBook;
using Application.BookQueries.GetBooks;
using Application.Types;
using Common.Domain;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class BookQueries
{
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public async Task<BookDto> GetBookById(Guid id, ISender sender)
    {
        return await sender.Send(new GetBookByIdQuery(id));
    }

    [UsePaging(MaxPageSize = 10)]
    [UseProjection]
    public async Task<IQueryable<BookDto>> GetBooks(ISender sender)
    {
        return await sender.Send(new GetBooksQuery());
    }
}