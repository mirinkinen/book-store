using Application.Types;
using Domain;
using GreenDonut.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BookQueries.GetBooks;

public record GetBooksQuery : IRequest<Page<Book>>
{
    public PagingArguments PagingArguments { get; }
    public QueryContext<Book> QueryContext { get; }

    public GetBooksQuery(PagingArguments pagingArguments, QueryContext<Book> queryContext)
    {
        PagingArguments = pagingArguments;
        QueryContext = queryContext;
    }
}

public class GetBooksHandler : IRequestHandler<GetBooksQuery, Page<Book>>
{
    private readonly IQueryRepository<Book> _queryRepository;

    public GetBooksHandler(IQueryRepository<Book> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public Task<Page<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return _queryRepository.With(request.PagingArguments, request.QueryContext).AsTask();
    }
}