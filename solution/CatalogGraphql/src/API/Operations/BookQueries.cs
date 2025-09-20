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
    public async Task<BookDto> GetBookById(Guid id, IMediator mediator)
    {
        return await mediator.Send(new GetBookByIdQuery(id));
    }

    [UsePaging(MaxPageSize = 10)]
    [UseProjection]
    public async Task<IQueryable<BookDto>> GetBooks(IMediator mediator)
    {
        return await mediator.Send(new GetBooksQuery());
    }
}