using Cataloging.Domain.Authors;
using Common.Application.Auditing;
using Common.Application.Authentication;
using System.Diagnostics.CodeAnalysis;
using Wolverine;
using Wolverine.Attributes;

namespace Cataloging.Application.Requests.Authors.AddAuthor;

public record AddAuthorCommand(string Firstname, string Lastname, DateTime Birthday, Guid OrganizationId, User Actor);

[Transactional]
public class AddAuthorHandler
{
    private readonly IAuthorRepository _authorRepository;

    public AddAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public async IAsyncEnumerable<object> Handle(AddAuthorCommand request)
    {
        var author = new Author(request.Firstname, request.Lastname, request.Birthday, request.OrganizationId);

        _authorRepository.AddAuthor(author);
        await _authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorAdded(request.Actor.Id);
        yield return new AuditLogEvent(request.Actor.Id, OperationType.Create, new[] { new AuditLogResource(author.Id, "Author")});
    }
}