using Books.Domain.Authors;
using MediatR;

namespace Books.Application.Requests.Authors.AddAuthor;

public record UpdateAuthorCommand(Guid AuthorId, string Firstname, string Lastname, DateTime Birthday)
    : IRequest<Author?>;

internal class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, Author?>
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<Author?> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetAuthorById(request.AuthorId, cancellationToken);

        if (author == null)
        {
            return null;
        }

        author.Update(request.Firstname, request.Lastname, request.Birthday);

        await _authorRepository.SaveChangesAsync();

        return author;
    }
}