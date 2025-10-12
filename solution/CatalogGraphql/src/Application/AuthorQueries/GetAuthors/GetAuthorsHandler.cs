using Domain;
using GreenDonut.Data;
using MediatR;

namespace Application.AuthorQueries.GetAuthors;

public record GetAuthorsQuery : IRequest<Page<AuthorNode>>
{
    public PagingArguments PagingArguments { get; }
    public QueryContext<AuthorNode> QueryContext { get; }

    public GetAuthorsQuery(PagingArguments pagingArguments, QueryContext<AuthorNode> queryContext)
    {
        PagingArguments = pagingArguments;
        QueryContext = queryContext;
    }
}

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, Page<AuthorNode>>
{
    private readonly IAuthorReadRepository _readRepository;

    public GetAuthorsHandler(IAuthorReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<Page<AuthorNode>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.GetPage(request.PagingArguments, request.QueryContext, cancellationToken).AsTask();
    }
}