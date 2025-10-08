using MediatR;

namespace Application.BookQueries.GetBookById;

public record GetBookByIdQuery(Guid Id) : IRequest<BookNode?>;

public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, BookNode?>
{
    private readonly IBookReadRepository _bookReadRepository;

    public GetBookByIdHandler(IBookReadRepository bookReadRepository)
    {
        _bookReadRepository = bookReadRepository;
    }

    public async Task<BookNode?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookReadRepository.FirstOrDefaultAsync(request.Id, cancellationToken);

        // if (book is null)
        // {
        //     throw new EntityNotFoundException("Book not found", "book-not-found");
        // }

        return book;
    }
}