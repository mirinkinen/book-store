using Books.Application.Auditing;
using Books.Application.Services;
using Books.Domain.Authors;
using MediatR;

namespace Books.Application.Requests.Authors.UpdateAuthor;

public record UpdateAuthorCommand(Guid AuthorId, string Firstname, string Lastname, DateTime Birthday, User Actor)
    : IAuditRequest<Author?>
{
    public OperationType OperationType => OperationType.Update;
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
        var author = await _authorRepository.GetAuthorById(request.AuthorId, cancellationToken);

        if (author == null)
        {
            return null;
        }

        var user = _userService.GetUser();
        author.Update(request.Firstname, request.Lastname, request.Birthday, user.Id);

        _auditContext.AddResource(ResourceType.Author, request.AuthorId);

        await _authorRepository.SaveChangesAsync();

        return author;
    }
}