using Application.AuthorQueries;
using Common.Domain;
using Domain;
using HotChocolate.Subscriptions;
using MediatR;

namespace Application.AuthorCommands.CreateAuthor;

public record CreateAuthorCommand(
    string FirstName,
    string LastName,
    DateOnly Birthdate,
    Guid OrganizationId) : IRequest<AuthorDto>;

public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IAuthorWriteRepository _authorWriteRepository;
    private readonly ITopicEventSender _eventSender;

    public CreateAuthorHandler(IAuthorWriteRepository authorWriteRepository, ITopicEventSender eventSender)
    {
        _authorWriteRepository = authorWriteRepository;
        _eventSender = eventSender;
    }
    
    public async Task<AuthorDto> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        var authorExists = await _authorWriteRepository.AuthorWithNameExists(command.FirstName, command.LastName, cancellationToken);
        if (authorExists)
        {
            throw new DomainRuleException("Author already exists with given name.", "author-already-exists-with-given-name");
        }
        
        var author = new Author(command.FirstName, command.LastName, command.Birthdate, command.OrganizationId);
        _authorWriteRepository.Add(author);
        await _authorWriteRepository.SaveChangesAsync(cancellationToken);
        
        
        await _eventSender.SendAsync(nameof(CreateAuthor), author, cancellationToken);

        return author.ToDto();
    }
}
