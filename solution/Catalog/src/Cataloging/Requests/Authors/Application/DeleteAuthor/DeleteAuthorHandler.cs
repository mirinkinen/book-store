using Cataloging.Requests.Authors.Domain;
using Common.Api.Application.Auditing;
using Common.Api.Application.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Requests.Authors.Application.DeleteAuthor;

public record DeleteAuthorCommand(Guid AuthorId, User Actor) : IAuthorCommand;

public class DeleteAuthorHandler
{
    private readonly IAuthorRepository _authorRepository;

    public DeleteAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public async IAsyncEnumerable<object> Handle(DeleteAuthorCommand request, Author author)
    {
        _authorRepository.Delete(author);
        await _authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorDeleted(author.Id);
        yield return new AuditLogEvent(request.Actor.Id, OperationType.Delete, new[] { new AuditLogResource(author.Id, "Author") });
    }
}