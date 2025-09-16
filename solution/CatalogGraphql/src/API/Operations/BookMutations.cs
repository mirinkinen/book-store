using Application.BookCommands.CreateBook;
using Application.BookCommands.DeleteBook;
using Application.BookCommands.UpdateBook;
using Application.Types;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class BookMutations
{
    public async Task<BookDto> CreateBook(CreateBookCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }

    public async Task<BookDto> UpdateBook(UpdateBookCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }

    public async Task<DeleteBookOutput> DeleteBook(DeleteBookCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }
}