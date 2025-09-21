using Application.Types;
using Common.Domain;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBook;

public record GetBookByIdQuery(Guid Id) : IRequest<Book>;

public class GetBookHandler : IRequestHandler<GetBookByIdQuery, Book>
{
    private readonly IBookWriteRepository _bookWriteRepository;

    public GetBookHandler(IBookWriteRepository bookWriteRepository)
    {
        _bookWriteRepository = bookWriteRepository;
    }

    public async Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookWriteRepository.FirstOrDefaultAsync(request.Id);

        if (book is null)
        {
            throw new EntityNotFoundException("Book");
        }

        return book;
    }
}