using Application.Types;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthor;

public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDto?>;

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