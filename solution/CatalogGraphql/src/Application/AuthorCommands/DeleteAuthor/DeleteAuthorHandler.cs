using Domain;
using MediatR;

namespace Application.AuthorCommands.DeleteAuthor;

public record DeleteAuthorCommand(Guid Id) : IRequest<DeleteAuthorOutput>;

public record DeleteAuthorOutput(bool Success, Guid Id);

public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, DeleteAuthorOutput>
{
    private readonly IAuthorRepository _authorRepository;

    public DeleteAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<DeleteAuthorOutput> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
    {
        var success = await _authorRepository.DeleteAsync(command.Id);

        return new DeleteAuthorOutput(success, command.Id);
    }
}
