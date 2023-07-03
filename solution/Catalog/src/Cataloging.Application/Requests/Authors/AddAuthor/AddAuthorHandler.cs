using Cataloging.Domain.Authors;
using Common.Application.Authentication;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.Application.Requests.Authors.AddAuthor;

public record AddAuthorCommand(string Firstname, string Lastname, DateTime Birthday, Guid OrganizationId, User Actor);

public class AddAuthorHandler
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMessageBus _bus;

    public AddAuthorHandler(IAuthorRepository authorRepository, IMessageBus bus)
    {
        _authorRepository = authorRepository;
        _bus = bus;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public async Task<Author> Handle(AddAuthorCommand request)
    {
        var author = new Author(request.Firstname, request.Lastname, request.Birthday, request.OrganizationId);

        _authorRepository.AddAuthor(author);
        await _authorRepository.SaveChangesAsync();

        var authorAddedEvent = new AuthorAdded(request.Actor.Id, author.Id, author.Birthday, author.FirstName, author.LastName, author.OrganizationId);
        await _bus.SendAsync(authorAddedEvent);

        return author;
    }
}