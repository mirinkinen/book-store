using Application.AuthorQueries.GetAuthor;
using Application.Repositories;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthors;

public class GetAuthorsInput : IRequest<IEnumerable<GetAuthorOutput>>
{
}

public class GetAuthorsHandler : IRequestHandler<GetAuthorsInput, IEnumerable<GetAuthorOutput>>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorsHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<IEnumerable<GetAuthorOutput>> Handle(GetAuthorsInput request, CancellationToken cancellationToken)
    {
        var authors = await _authorRepository.GetAllAsync();
        
        var authorOutputs = authors.Select(author => new GetAuthorOutput
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
