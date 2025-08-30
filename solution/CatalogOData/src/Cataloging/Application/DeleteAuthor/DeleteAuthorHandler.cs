using Cataloging.Application.Auditing;
using Cataloging.Domain;
using Common.Application.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Application.DeleteAuthor;

public record DeleteAuthorCommand(Guid AuthorId) : IAuthorCommand;

public static class DeleteAuthorHandler
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public static async IAsyncEnumerable<object> Handle(DeleteAuthorCommand request, Author author, IAuthorRepository authorRepository,
        IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        authorRepository.Delete(author);
        await authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorDeleted(author.Id);
        yield return new AuditLogEvent(user.Id, OperationType.Delete, new[] { new AuditLogResource(author.Id, "Author") });
    }
}