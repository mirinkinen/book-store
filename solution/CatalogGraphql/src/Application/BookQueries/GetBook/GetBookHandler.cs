using Application.Types;
using Domain;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BookQueries.GetBook;

public class GetBookByIdQuery : IRequest<BookOutputType?>
{
    [SetsRequiredMembers]
    public GetBookByIdQuery(Guid id)
    {
        Id = id;
    }

    public required Guid Id { get; set; }
}

public class GetBookHandler : IRequestHandler<GetBookByIdQuery, BookOutputType?>
{
    private readonly IBookRepository _bookRepository;

    public GetBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookOutputType?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);

        if (book == null)
            return null;

        return new BookOutputType
        {
            Id = book.Id,
            AuthorId = book.AuthorId,
            Title = book.Title,
            DatePublished = book.DatePublished,
            Price = book.Price
        };
    }
}