using Cataloging.Domain;
using Common.Application.Auditing;
using Common.Application.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Application.UpdateAuthor;

public record UpdateAuthorCommand(Guid AuthorId, DateTime Birthday, string FirstName, string LastName) : IAuthorCommand;

public record AuthorUpdated(Guid AuthorId);

public static class UpdateAuthorHandler
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public static async IAsyncEnumerable<object> Handle(UpdateAuthorCommand request, Author author, IAuthorRepository authorRepository,
        IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();

        author.Update(request.FirstName, request.LastName, request.Birthday);

        await authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorUpdated(author.Id);
        yield return new AuditLogEvent(user.Id, OperationType.Update, new[] { new AuditLogResource(author.Id, "Author") });
    }
}