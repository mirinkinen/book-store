using Books.Domain.Authors;
using MediatR;

namespace Books.Application.Requests.Authors.AddAuthor;

public record AddAuthorCommand(string Firstname, string Lastname, DateTime Birthday, Guid OrganizationId)
    : IRequest<Author>;

internal class AddAuthorHandler : IRequestHandler<AddAuthorCommand, Author>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUserService _userService;

    public AddAuthorHandler(IAuthorRepository authorRepository, IUserService userService)
    {
        _authorRepository = authorRepository;
        _userService = userService;
    }

    public async Task<Author> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
    {
        var user = _userService.GetUser();
        var author = new Author(request.Firstname, request.Lastname, request.Birthday, request.OrganizationId, user.Id);

        _authorRepository.AddAuthor(author);
        await _authorRepository.SaveChangesAsync();

        return author;
    }
}