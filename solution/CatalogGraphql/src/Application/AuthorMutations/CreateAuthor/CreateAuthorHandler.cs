using Application.Repositories;
using Domain;
using MediatR;

namespace Application.AuthorMutations.CreateAuthor;

public class CreateAuthorInput : IRequest<AuthorCreatedPayload>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
    public required Guid OrganizationId { get; set; }
}

public class AuthorCreatedPayload : IRequest<Author>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
    public required Guid OrganizationId { get; set; }
}

public class CreateAuthorHandler : IRequestHandler<CreateAuthorInput, AuthorCreatedPayload>
{
    private readonly IAuthorRepository _authorRepository;

    public CreateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<AuthorCreatedPayload> Handle(CreateAuthorInput request, CancellationToken cancellationToken)
    {
        var author = new Author(request.FirstName, request.LastName, request.Birthdate, request.OrganizationId);
        
        await _authorRepository.AddAsync(author);

        return new AuthorCreatedPayload
        {
            FirstName = author.FirstName,
            LastName = author.LastName,
            Birthdate = author.Birthdate,
            OrganizationId = author.OrganizationId
        };
    }
}
