using Application.Types;
using Domain;
using GreenDonut.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BookQueries.GetBooks;

public record GetBooksQuery : IRequest<IQueryable<Book>>
{
    public QueryContext<Book> QueryContext { get; }

    public GetBooksQuery(QueryContext<Book> queryContext)
    {
        QueryContext = queryContext;
    }
}

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IQueryable<Book>>
{
    private readonly IQueryRepository<Book> _queryRepository;

    public GetBooksHandler(IQueryRepository<Book> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public Task<IQueryable<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = _queryRepository.With(request.QueryContext).OrderBy(e => e.Id).Select(e => e);

        return Task.FromResult(books);
    }
}