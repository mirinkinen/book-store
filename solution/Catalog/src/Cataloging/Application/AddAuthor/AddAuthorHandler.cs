using Cataloging.Domain;
using Common.Application.Auditing;
using Common.Application.Authentication;
using System.Diagnostics.CodeAnalysis;
using Wolverine.Attributes;

namespace Cataloging.Application.AddAuthor;

public record PostAuthorCommand(string Firstname, string Lastname, DateTime Birthday, Guid OrganizationId);

[Transactional]
public static class AddAuthorHandler
{

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public static async IAsyncEnumerable<object> Handle(PostAuthorCommand request, IAuthorRepository authorRepository, IUserAccessor 
        userAccessor)
    {
        var user = await userAccessor.GetUser();
        var author = new Author(request.Firstname, request.Lastname, request.Birthday, request.OrganizationId);

        authorRepository.AddAuthor(author);
        await authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorAdded(user.Id);
        yield return new AuditLogEvent(user.Id, OperationType.Create, new[] { new AuditLogResource(author.Id, "Author") });
    }
}