using Application.Types;
using Domain;
using MediatR;

namespace Application.AuthorCommands.CreateAuthor;

public record CreateAuthorCommand(
    string FirstName,
    string LastName,
    DateTime Birthdate,
    Guid OrganizationId) : IRequest<AuthorDto>;

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
