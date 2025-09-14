using Application.Repositories;
using MediatR;

namespace Application.AuthorMutations.DeleteAuthor;

public class DeleteAuthorInput : IRequest<DeleteAuthorOutput>
{
    public required Guid Id { get; set; }
}

public class DeleteAuthorOutput
{
    public required bool Success { get; set; }
    public required Guid Id { get; set; }
}

public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorInput, DeleteAuthorOutput>
{
    private readonly IAuthorRepository _authorRepository;

    public DeleteAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<DeleteAuthorOutput> Handle(DeleteAuthorInput input, CancellationToken cancellationToken)
    {
        var success = await _authorRepository.DeleteAsync(input.Id);
        
        return new DeleteAuthorOutput
        {
            Success = success,
            Id = input.Id
        };
    }
}
