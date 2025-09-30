using GreenDonut.Data;
using MediatR;

namespace Application.BookQueries.GetBooks;

public record GetBooksQuery : IRequest<Page<BookNode>>
{
    public PagingArguments PagingArguments { get; }
    public QueryContext<BookNode> QueryContext { get; }

    public GetBooksQuery(PagingArguments pagingArguments, QueryContext<BookNode> queryContext)
    {
        PagingArguments = pagingArguments;
        QueryContext = queryContext;
    }
}

public class GetBooksHandler : IRequestHandler<GetBooksQuery, Page<BookNode>>
{
    private readonly IBookReadRepository _readRepository;

    public GetBooksHandler(IBookReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<Page<BookNode>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.With(request.PagingArguments, request.QueryContext, cancellationToken).AsTask();
    }
}