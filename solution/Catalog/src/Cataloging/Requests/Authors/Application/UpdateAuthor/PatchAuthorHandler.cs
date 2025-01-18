using Cataloging.Requests.Authors.Domain;
using Common.Application.Auditing;
using Common.Application.Authentication;
using Microsoft.AspNetCore.OData.Deltas;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Requests.Authors.Application.UpdateAuthor;

public record PatchAuthorCommand(Guid AuthorId, Delta<Author> delta, User Actor) : 
    IAuthorCommand;

public class PatchAuthorHandler
{
    private readonly IAuthorRepository _authorRepository;

    public PatchAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public async IAsyncEnumerable<object> Handle(PatchAuthorCommand request, Author author)
    {
        author.Patch(request.delta);

        await _authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorUpdated(author.Id);
        yield return new AuditLogEvent(request.Actor.Id, OperationType.Update, new[] { new AuditLogResource(author.Id, "Author") });
    }
}