using Books.Domain.Authors;
using MediatR;

namespace Books.Application.Requests.Authors.UpdateAuthor;

public record UpdateAuthorCommand(Guid AuthorId, string Firstname, string Lastname, DateTime Birthday)
    : IRequest<Author?>;

internal class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, Author?>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUserService _userService;

    public UpdateAuthorHandler(IAuthorRepository authorRepository, IUserService userService)
    {
        _authorRepository = authorRepository;
        _userService = userService;
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

        await _authorRepository.SaveChangesAsync();

        return author;
    }
}