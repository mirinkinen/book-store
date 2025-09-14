using Application.BookQueries.GetBook;
using Application.BookQueries.GetBooks;
using Application.BookQueries.GetBooksByAuthor;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class BookQueries
{
    public async Task<GetBookOutput?> GetBook(GetBookInput input, IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<IEnumerable<GetBookOutput>> GetBooks(IMediator mediator)
    {
        return await mediator.Send(new GetBooksInput());
    }

    public async Task<IEnumerable<GetBookOutput>> GetBooksByAuthor(GetBooksByAuthorInput input, IMediator mediator)
    {
        return await mediator.Send(input);
    }

}