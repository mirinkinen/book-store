using Cataloging.Domain.Authors;

namespace Cataloging.Application.Requests.Authors.AddAuthor;

public record AddAuthorCommand(string Firstname, string Lastname, DateTime Birthday, Guid OrganizationId);

internal class AddAuthorHandler
{
    private readonly IAuthorRepository _authorRepository;

    public AddAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<Author> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = new Author(request.Firstname, request.Lastname, request.Birthday, request.OrganizationId);

        _authorRepository.AddAuthor(author);
        await _authorRepository.SaveChangesAsync();

        return author;
    }
}