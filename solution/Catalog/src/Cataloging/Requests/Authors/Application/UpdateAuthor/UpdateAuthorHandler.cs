using Cataloging.Requests.Authors.Domain;
using Common.Application.Auditing;
using Common.Application.Authentication;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Requests.Authors.Application.UpdateAuthor;

public record UpdateAuthorCommand(Guid AuthorId, AuthorPutDtoV1 Dto, User Actor) : IAuthorCommand;

public record AuthorUpdated(Guid AuthorId);

public class UpdateAuthorHandler
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public static async IAsyncEnumerable<object> Handle(UpdateAuthorCommand request, Author author, IAuthorRepository authorRepository,
        IValidator<AuthorPutDtoV1> authorPutValidator)
    {
        await authorPutValidator.ValidateAndThrowAsync(request.Dto);

        author.Update(request.Dto.FirstName, request.Dto.LastName, request.Dto.Birthday);

        await authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorUpdated(author.Id);
        yield return new AuditLogEvent(request.Actor.Id, OperationType.Update, new[] { new AuditLogResource(author.Id, "Author") });
    }
}