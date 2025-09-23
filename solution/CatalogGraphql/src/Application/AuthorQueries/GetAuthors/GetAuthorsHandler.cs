using Domain;
using GreenDonut.Data;
using MediatR;

namespace Application.AuthorQueries.GetAuthors;

public record GetAuthorsQuery : IRequest<Page<Author>>
{
    public PagingArguments PagingArguments { get; }
    public QueryContext<Author> QueryContext { get; }

    public GetAuthorsQuery(PagingArguments pagingArguments, QueryContext<Author> queryContext)
    {
        PagingArguments = pagingArguments;
        QueryContext = queryContext;
    }
}

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, Page<Author>>
{
    private readonly IQueryRepository<Author> _queryRepository;

    public GetAuthorsHandler(IQueryRepository<Author> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public Task<Page<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return _queryRepository.With(request.PagingArguments, request.QueryContext).AsTask();
    }
}