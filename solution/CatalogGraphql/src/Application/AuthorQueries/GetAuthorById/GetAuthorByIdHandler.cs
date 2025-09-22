using Common.Domain;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthorById;

public record GetAuthorByIdQuery(Guid Id) : IRequest<Author>;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, Author>
{
    private readonly IReadRepository<Author> _readRepository;

    public GetAuthorByIdHandler(IReadRepository<Author> readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _readRepository.FirstOrDefaultAsync(request.Id, cancellationToken);

        if (author is null)
        {
            throw new EntityNotFoundException("Author not found", "author-not-found");
        }

        return author;
    }
}