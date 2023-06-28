using Cataloging.Domain.Authors;
using MediatR;

namespace Cataloging.Application.Requests.Authors.DeleteAuthor;

public record DeleteAuthorCommand(Guid AuthorId) : IRequest<Author?>;

internal class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, Author?>
{
    private readonly IAuthorRepository _authorRepository;

    public DeleteAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<Author?> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetAuthorById(request.AuthorId, cancellationToken);

        if (author == null)
        {
            return null;
        }

        _authorRepository.Delete(author);
        await _authorRepository.SaveChangesAsync();

        return author;
    }
}