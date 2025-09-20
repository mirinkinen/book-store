using Application.Types;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthors;

public record GetAuthorsQuery : IRequest<IQueryable<AuthorDto>>;

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, IQueryable<AuthorDto>>
{
    private readonly IReadRepository<Author> _readRepository;

    public GetAuthorsHandler(IReadRepository<Author> readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<IQueryable<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = _readRepository.GetQuery();

        return Task.FromResult(authors.Select(author => author.ToDto()));
    }
}