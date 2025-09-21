using Application.Types;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBooks;

public record GetBooksQuery : IRequest<IQueryable<BookDto>>;

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IQueryable<BookDto>>
{
    private readonly IQueryRepository<Book> _queryRepository;

    public GetBooksHandler(IQueryRepository<Book> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public Task<IQueryable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = _queryRepository.GetQuery();

        return Task.FromResult(books.Select(book => book.ToDto()));
    }
}