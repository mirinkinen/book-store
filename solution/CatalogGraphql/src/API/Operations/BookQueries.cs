using Application.BookMutations.CreateBook;
using Application.BookQueries.GetBook;
using Application.BookQueries.GetBooks;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class BookQueries
{
    public async Task<BookOutputType?> GetBookById(Guid id, IMediator mediator)
    {
        return await mediator.Send(new GetBookByIdQuery(id));
    }

    public async Task<IEnumerable<BookOutputType>> GetBooks(IMediator mediator)
    {
        return await mediator.Send(new GetBooksQuery());
    }
}