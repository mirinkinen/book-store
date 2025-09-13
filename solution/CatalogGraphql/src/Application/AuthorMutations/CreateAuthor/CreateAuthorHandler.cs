using Application.Repositories;
using Domain;
using MediatR;

namespace Application.AuthorMutations.CreateAuthor;

public class CreateAuthorInput : IRequest<AuthorCreatedOutput>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
    public required Guid OrganizationId { get; set; }
}

public class AuthorCreatedOutput : IRequest<Author>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
    public required Guid OrganizationId { get; set; }
}

public class CreateAuthorHandler : IRequestHandler<CreateAuthorInput, AuthorCreatedOutput>
{
    private readonly IAuthorRepository _authorRepository;

    public CreateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<AuthorCreatedOutput> Handle(CreateAuthorInput input, CancellationToken cancellationToken)
    {
        var author = new Author(input.FirstName, input.LastName, input.Birthdate, input.OrganizationId);
        
        await _authorRepository.AddAsync(author);

        return new AuthorCreatedOutput
        {
            FirstName = author.FirstName,
            LastName = author.LastName,
            Birthdate = author.Birthdate,
            OrganizationId = author.OrganizationId
        };
    }
}
