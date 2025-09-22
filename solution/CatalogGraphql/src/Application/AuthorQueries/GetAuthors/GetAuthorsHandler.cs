using Application.Types;
using Domain;
using GreenDonut.Data;
using MediatR;

namespace Application.AuthorQueries.GetAuthors;

public record GetAuthorsQuery : IRequest<IQueryable<Author>>
{
    public QueryContext<Author> QueryContext { get; }

    public GetAuthorsQuery(QueryContext<Author> queryContext)
    {
        QueryContext = queryContext;
    }
}

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, IQueryable<Author>>
{
    private readonly IQueryRepository<Author> _queryRepository;

    public GetAuthorsHandler(IQueryRepository<Author> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public Task<IQueryable<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = _queryRepository.With(request.QueryContext);

        return Task.FromResult(authors);
    }
}