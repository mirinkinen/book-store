using Domain;
using MediatR;

namespace Application.AuthorCommands.DeleteAuthor;

public record DeleteAuthorCommand(Guid Id) : IRequest<DeleteAuthorPayload>;

public record DeleteAuthorPayload(bool Success, Guid Id);

public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, DeleteAuthorPayload>
{
    private readonly IAuthorRepository _authorRepository;

    public DeleteAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<DeleteAuthorPayload> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
    {
        var success = await _authorRepository.DeleteAsync(command.Id);

        return new DeleteAuthorPayload(success, command.Id);
    }
}
