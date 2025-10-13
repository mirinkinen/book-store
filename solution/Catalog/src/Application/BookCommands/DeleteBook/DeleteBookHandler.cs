using Domain.Books;
using MediatR;

namespace Application.BookCommands.DeleteBook;

public record DeleteBookCommand(Guid Id) : IRequest<DeleteBookPayload>;

public record DeleteBookPayload(bool Success, Guid Id);

public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, DeleteBookPayload>
{
    private readonly IBookWriteRepository _bookWriteRepository;

    public DeleteBookHandler(IBookWriteRepository bookWriteRepository)
    {
        _bookWriteRepository = bookWriteRepository;
    }

    public async Task<DeleteBookPayload> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
    {
        var success = await _bookWriteRepository.DeleteAsync(command.Id);

        return new DeleteBookPayload(success, command.Id);
    }
}