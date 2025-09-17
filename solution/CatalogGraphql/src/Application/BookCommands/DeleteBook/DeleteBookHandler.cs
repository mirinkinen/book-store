using Domain;
using MediatR;

namespace Application.BookCommands.DeleteBook;

public record DeleteBookCommand(Guid Id) : IRequest<DeleteBookPayload>;

public record DeleteBookPayload(bool Success, Guid Id);

public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, DeleteBookPayload>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<DeleteBookPayload> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
    {
        var success = await _bookRepository.DeleteAsync(command.Id);

        return new DeleteBookPayload(success, command.Id);
    }
}
