using Application.Types;
using Domain;
using MediatR;

namespace Application.AuthorCommands.CreateAuthor;

public class CreateAuthorCommand : IRequest<AuthorDto>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
    public required Guid OrganizationId { get; set; }
}

public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IAuthorRepository _authorRepository;

    public CreateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<AuthorDto> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = new Author(command.FirstName, command.LastName, command.Birthdate, command.OrganizationId);
        
        await _authorRepository.AddAsync(author);

        return author.ToDto();
    }
}
