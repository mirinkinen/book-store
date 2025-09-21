using Application.Types;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthors;

public record GetAuthorsQuery : IRequest<IQueryable<AuthorDto>>;

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, IQueryable<AuthorDto>>
{
    private readonly IQueryRepository<Author> _queryRepository;

    public GetAuthorsHandler(IQueryRepository<Author> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public Task<IQueryable<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = _queryRepository.GetQuery();

        return Task.FromResult(authors.Select(author => author.ToDto()));
    }
}