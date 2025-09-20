using Application.Types;
using Common.Domain;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var authorExists = await _authorRepository.GetQuery()
            .AnyAsync(a => a.FirstName == command.FirstName && a.LastName == command.LastName, CancellationToken.None);

        if (authorExists)
        {
            throw new DomainRuleException("Author already exists with given name.", "author-already-exists-with-given-name");
        }
        
        var author = new Author(command.FirstName, command.LastName, command.Birthdate, command.OrganizationId);
        
        await _authorRepository.AddAsync(author);

        return author.ToDto();
    }
}
