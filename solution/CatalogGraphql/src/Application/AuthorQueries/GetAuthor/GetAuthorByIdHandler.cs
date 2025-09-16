using Application.AuthorCommands;
using Application.Types;
using Domain;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.AuthorQueries.GetAuthor;

public class GetAuthorByIdQuery : IRequest<AuthorDto?>
{
    [SetsRequiredMembers]
    public GetAuthorByIdQuery(Guid id)
    {
        Id = id;
    }

    public required Guid Id { get; set; }
}

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto?>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorByIdHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorDto?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(request.Id);

        if (author == null)
            return null;

        return author.ToDto();
    }
}