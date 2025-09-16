using Application.BookQueries.GetBook;
using Application.BookQueries.GetBooks;
using Application.Types;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class BookQueries
{
    public async Task<BookDto?> GetBookById(Guid id, IMediator mediator)
    {
        return await mediator.Send(new GetBookByIdQuery(id));
    }

    public async Task<IEnumerable<BookDto>> GetBooks(IMediator mediator)
    {
        return await mediator.Send(new GetBooksQuery());
    }
}