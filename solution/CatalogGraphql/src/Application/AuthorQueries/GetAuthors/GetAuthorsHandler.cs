using Application.Types;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthors;

public record GetAuthorsQuery : IRequest<IQueryable<AuthorDto>>;

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, IQueryable<AuthorDto>>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorsHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IQueryable<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _authorRepository.GetAllAsync();

        return authors.Select(author => author.ToDto());
    }
}