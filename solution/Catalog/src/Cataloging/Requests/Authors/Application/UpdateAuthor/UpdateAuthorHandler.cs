using Cataloging.Requests.Authors.Domain;
using Common.Api.Application.Auditing;
using Common.Api.Application.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Requests.Authors.Application.UpdateAuthor;

public record UpdateAuthorCommand(Guid AuthorId, string Firstname, string Lastname, DateTime Birthday, User Actor) : IAuthorCommand;

public class UpdateAuthorHandler
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public async IAsyncEnumerable<object> Handle(UpdateAuthorCommand request, Author author)
    {
        author.Update(request.Firstname, request.Lastname, request.Birthday);

        await _authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorUpdated(author.Id);
        yield return new AuditLogEvent(request.Actor.Id, OperationType.Update, new[] { new AuditLogResource(author.Id, "Author") });
    }
}