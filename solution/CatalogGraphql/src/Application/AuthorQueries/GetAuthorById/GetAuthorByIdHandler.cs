using Application.AuthorQueries.GetAuthors;
using Common.Domain;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthorById;

public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDto>;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    private readonly IAuthorReadRepository _readRepository;

    public GetAuthorByIdHandler(IAuthorReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _readRepository.FirstOrDefaultAsync(request.Id, cancellationToken);

        if (author is null)
        {
            throw new EntityNotFoundException("Author not found", "author-not-found");
        }

        return author;
    }
}