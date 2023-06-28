using Cataloging.Application.Auditing;
using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using MediatR;

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
    private readonly IUserService _userService;
    private readonly IAuditContext _auditContext;

    public UpdateAuthorHandler(IAuthorRepository authorRepository, IUserService userService, IAuditContext auditContext)
    {
        _authorRepository = authorRepository;
        _userService = userService;
        _auditContext = auditContext;
    }

    public async Task<Author?> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetAuthorById(request.ResourceId, cancellationToken);

        if (author == null)
        {
            return null;
        }

        var user = _userService.GetUser();
        author.Update(request.Firstname, request.Lastname, request.Birthday, user.Id);

        await _authorRepository.SaveChangesAsync();

        return author;
    }
}