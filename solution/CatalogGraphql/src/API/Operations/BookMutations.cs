using Application.BookMutations.CreateBook;
using Application.BookMutations.DeleteBook;
using Application.BookMutations.UpdateBook;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class BookMutations
{
    public async Task<BookOutputType> CreateBook(CreateBookCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }

    public async Task<BookOutputType> UpdateBook(UpdateBookCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }

    public async Task<DeleteBookOutput> DeleteBook(DeleteBookCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }
}