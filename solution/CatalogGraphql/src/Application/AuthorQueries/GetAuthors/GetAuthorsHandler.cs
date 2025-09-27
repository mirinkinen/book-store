using GreenDonut.Data;
using MediatR;

namespace Application.AuthorQueries.GetAuthors;

public record GetAuthorsQuery : IRequest<Page<AuthorDto>>
{
    public PagingArguments PagingArguments { get; }
    public QueryContext<AuthorDto> QueryContext { get; }

    public GetAuthorsQuery(PagingArguments pagingArguments, QueryContext<AuthorDto> queryContext)
    {
        PagingArguments = pagingArguments;
        QueryContext = queryContext;
    }
}

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, Page<AuthorDto>>
{
    private readonly IAuthorReadRepository _readRepository;

    public GetAuthorsHandler(IAuthorReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<Page<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.With(request.PagingArguments, request.QueryContext, cancellationToken).AsTask();
    }
}