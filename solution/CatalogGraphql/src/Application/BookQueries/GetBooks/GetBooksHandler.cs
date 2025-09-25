using GreenDonut.Data;
using MediatR;

namespace Application.BookQueries.GetBooks;

public record GetBooksQuery : IRequest<Page<BookDto>>
{
    public PagingArguments PagingArguments { get; }
    public QueryContext<BookDto> QueryContext { get; }

    public GetBooksQuery(PagingArguments pagingArguments, QueryContext<BookDto> queryContext)
    {
        PagingArguments = pagingArguments;
        QueryContext = queryContext;
    }
}

public class GetBooksHandler : IRequestHandler<GetBooksQuery, Page<BookDto>>
{
    private readonly IBookReadRepository _readRepository;

    public GetBooksHandler(IBookReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<Page<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.With(request.PagingArguments, request.QueryContext).AsTask();
    }
}