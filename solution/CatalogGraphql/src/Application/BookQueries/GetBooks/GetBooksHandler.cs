using Application.Types;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBooks;

public record GetBooksQuery : IRequest<IQueryable<Book>>;

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IQueryable<Book>>
{
    private readonly IQueryRepository<Book> _queryRepository;

    public GetBooksHandler(IQueryRepository<Book> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public Task<IQueryable<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = _queryRepository.GetQuery();

        return Task.FromResult(books);
    }
}