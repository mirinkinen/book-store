using Application.Types;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBook;

public record GetBookByIdQuery(Guid Id) : IRequest<BookDto?>;

public class GetBookHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    private readonly IBookRepository _bookRepository;

    public GetBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);

        return book?.ToDto();
    }
}