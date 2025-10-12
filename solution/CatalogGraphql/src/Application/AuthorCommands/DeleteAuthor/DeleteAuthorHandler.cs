using Domain.Authors;
using MediatR;

namespace Application.AuthorCommands.DeleteAuthor;

public record DeleteAuthorCommand(Guid Id) : IRequest<DeleteAuthorPayload>;

public record DeleteAuthorPayload(bool Success, Guid Id);

public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, DeleteAuthorPayload>
{
    private readonly IAuthorWriteRepository _authorWriteRepository;

    public DeleteAuthorHandler(IAuthorWriteRepository authorWriteRepository)
    {
        _authorWriteRepository = authorWriteRepository;
    }

    public async Task<DeleteAuthorPayload> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
    {
        var success = await _authorWriteRepository.DeleteAsync(command.Id);

        return new DeleteAuthorPayload(success, command.Id);
    }
}