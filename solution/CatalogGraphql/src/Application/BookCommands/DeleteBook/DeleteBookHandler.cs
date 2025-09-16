using Domain;
using MediatR;

namespace Application.BookCommands.DeleteBook;

public record DeleteBookCommand(Guid Id) : IRequest<DeleteBookOutput>;

public record DeleteBookOutput(bool Success, Guid Id);

public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, DeleteBookOutput>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<DeleteBookOutput> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
    {
        var success = await _bookRepository.DeleteAsync(command.Id);

        return new DeleteBookOutput(success, command.Id);
    }
}
