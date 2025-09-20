using Domain;
using MediatR;

namespace Application.AuthorCommands.MediatorHandlerWithMultipleRepositories;

public class MutationWithMultipleRepositoriesCommand : IRequest<string>
{
    
}

public class MutationWithMultipleRepositoriesHandler : IRequestHandler<MutationWithMultipleRepositoriesCommand, string>
{
    private readonly IBookWriteRepository _bookWriteRepository;
    private readonly IAuthorWriteRepository _authorWriteRepository;

    public MutationWithMultipleRepositoriesHandler(IBookWriteRepository bookWriteRepository, IAuthorWriteRepository authorWriteRepository)
    {
        _bookWriteRepository = bookWriteRepository;
        _authorWriteRepository = authorWriteRepository;
    }
    
    public Task<string> Handle(MutationWithMultipleRepositoriesCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult("Just testing");
    }
}