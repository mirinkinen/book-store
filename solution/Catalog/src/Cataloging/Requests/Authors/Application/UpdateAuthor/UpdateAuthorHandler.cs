using Cataloging.Requests.Authors.Domain;
using Common.Application.Auditing;
using Common.Application.Authentication;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Requests.Authors.Application.UpdateAuthor;

public record UpdateAuthorCommand(Guid AuthorId, PutAuthorDtoV1 Dto) : IAuthorCommand;

public record AuthorUpdated(Guid AuthorId);

public class UpdateAuthorHandler
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public static async IAsyncEnumerable<object> Handle(UpdateAuthorCommand request, Author author, IAuthorRepository authorRepository,
        IUserService userService)
    {
        var user = await userService.GetUser();

        author.Update(request.Dto.FirstName, request.Dto.LastName, request.Dto.Birthday);

        await authorRepository.SaveChangesAsync();

        yield return author;
        yield return new AuthorUpdated(author.Id);
        yield return new AuditLogEvent(user.Id, OperationType.Update, new[] { new AuditLogResource(author.Id, "Author") });
    }
}