using Domain;
using MediatR;

namespace Application.AuthorCommands.MediatorHandlerWithMultipleRepositories;

public class MutationWithMultipleRepositoriesCommand : IRequest<string>
{
    
}

public class MutationWithMultipleRepositoriesHandler : IRequestHandler<MutationWithMultipleRepositoriesCommand, string>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public MutationWithMultipleRepositoriesHandler(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }
    
    public Task<string> Handle(MutationWithMultipleRepositoriesCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult("Just testing");
    }
}