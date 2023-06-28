using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using MediatR;

namespace Cataloging.Application.Requests.Authors.AddAuthor;

public record AddAuthorCommand(string Firstname, string Lastname, DateTime Birthday, Guid OrganizationId)
    : IRequest<Author>;

internal class AddAuthorHandler : IRequestHandler<AddAuthorCommand, Author>
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