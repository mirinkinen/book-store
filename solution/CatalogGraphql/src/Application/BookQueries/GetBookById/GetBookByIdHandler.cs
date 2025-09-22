using Common.Domain;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBookById;

public record GetBookByIdQuery(Guid Id) : IRequest<Book>;

public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, Book>
{
    private readonly IReadRepository<Book> _bookReadRepository;

    public GetBookByIdHandler(IReadRepository<Book> bookReadRepository)
    {
        _bookReadRepository = bookReadRepository;
    }

    public async Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookReadRepository.FirstOrDefaultAsync(request.Id, cancellationToken);

        if (book is null)
        {
            throw new EntityNotFoundException("Book not found", "book-not-found");
        }

        return book;
    }
}