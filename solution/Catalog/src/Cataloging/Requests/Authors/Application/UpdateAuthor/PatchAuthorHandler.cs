using Cataloging.Requests.Authors.Domain;
using Common.Application.Auditing;
using Common.Application.Authentication;
using Microsoft.AspNetCore.OData.Deltas;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Requests.Authors.Application.UpdateAuthor;

public record PatchAuthorCommand(Guid AuthorId, Delta<Author> delta) : 
    IAuthorCommand;

public static class PatchAuthorHandler
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public static async IAsyncEnumerable<object> Handle(PatchAuthorCommand request, Author author, IAuthorRepository authorRepository, 
    IUserService userService)
    {
        var user = userService.GetUser();
        
        author.Patch(request.delta);

        await authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorUpdated(author.Id);
        yield return new AuditLogEvent(user.Id, OperationType.Update, new[] { new AuditLogResource(author.Id, "Author") });
    }
}