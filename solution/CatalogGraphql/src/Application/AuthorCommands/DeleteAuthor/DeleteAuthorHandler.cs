using Domain;
using MediatR;

namespace Application.AuthorCommands.DeleteAuthor;

public class DeleteAuthorCommand : IRequest<DeleteAuthorOutput>
{
    public required Guid Id { get; set; }
}

public class DeleteAuthorOutput
{
    public required bool Success { get; set; }
    public required Guid Id { get; set; }
}

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
        
        return new DeleteAuthorOutput
        {
            Success = success,
            Id = command.Id
        };
    }
}
