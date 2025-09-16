using Application.Types;
using Domain;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BookQueries.GetBook;

public class GetBookByIdQuery : IRequest<BookDto?>
{
    [SetsRequiredMembers]
    public GetBookByIdQuery(Guid id)
    {
        Id = id;
    }

    public required Guid Id { get; set; }
}

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