using Cataloging.Domain.Authors;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Application.Requests.Authors.DeleteAuthor;

public record DeleteAuthorCommand(Guid AuthorId);

public class DeleteAuthorHandler
{
    private readonly IAuthorRepository _authorRepository;

    public DeleteAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
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