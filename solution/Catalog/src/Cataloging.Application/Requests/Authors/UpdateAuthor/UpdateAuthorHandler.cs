using Cataloging.Domain.Authors;
using MediatR;
using Shared.Application.Auditing;
using Shared.Application.Authentication;

namespace Cataloging.Application.Requests.Authors.UpdateAuthor;

public record UpdateAuthorCommand(Guid ResourceId, string Firstname, string Lastname, DateTime Birthday, User Actor)
    : IAuditableCommand<Author?>
{
    public OperationType OperationType => OperationType.Update;

    public ResourceType ResourceType => ResourceType.Author;
}

internal class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, Author?>
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

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