using Application.AuthorCommands;
using Application.AuthorQueries.GetAuthor;
using Application.Types;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthors;

public class GetAuthorsQuery : IRequest<IEnumerable<AuthorOutputType>>
{
}

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<AuthorOutputType>>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorsHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<IEnumerable<AuthorOutputType>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _authorRepository.GetAllAsync();
        
        var authorOutputs = authors.Select(author => new AuthorOutputType
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Birthdate = author.Birthdate,
            OrganizationId = author.OrganizationId
        });

        return authorOutputs;
    }
}
