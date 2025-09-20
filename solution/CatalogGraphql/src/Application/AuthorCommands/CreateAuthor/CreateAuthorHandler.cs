using Application.Types;
using Common.Domain;
using Domain;
using HotChocolate.Subscriptions;
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
    private readonly ITopicEventSender _eventSender;

    public CreateAuthorHandler(IAuthorRepository authorRepository, ITopicEventSender eventSender)
    {
        _authorRepository = authorRepository;
        _eventSender = eventSender;
    }
    
    public async Task<AuthorDto> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        var authorExists = await _authorRepository.AuthorWithNameExists(command.FirstName, command.LastName, cancellationToken);
        if (authorExists)
        {
            throw new DomainRuleException("Author already exists with given name.", "author-already-exists-with-given-name");
        }
        
        var author = new Author(command.FirstName, command.LastName, command.Birthdate, command.OrganizationId);
        _authorRepository.Add(author);
        await _authorRepository.SaveChangesAsync();
        
        
        await _eventSender.SendAsync(nameof(CreateAuthor), author, cancellationToken);

        return author.ToDto();
    }
}
