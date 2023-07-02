using Cataloging.Domain.Authors;
using Shared.Application.Auditing;
using Shared.Application.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Application.Requests.Authors.UpdateAuthor;

public record UpdateAuthorCommand(Guid ResourceId, string Firstname, string Lastname, DateTime Birthday, User Actor)
    : IAuditableCommand
{
    public OperationType OperationType => OperationType.Update;

    public ResourceType ResourceType => ResourceType.Author;
}

public class UpdateAuthorHandler
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Never null")]
    public async Task<Author?> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetAuthorById(request.ResourceId, cancellationToken);

        if (author == null)
        {
            return null;
        }

        author.Update(request.Firstname, request.Lastname, request.Birthday);

        await _authorRepository.SaveChangesAsync();

        return author;
    }
}