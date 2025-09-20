using Application.Types;
using Common.Domain;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AuthorQueries.GetAuthor;

public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDto>;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    private readonly IReadRepository<Author> _readRepository;

    public GetAuthorByIdHandler(IReadRepository<Author> readRepository)
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

        return author.ToDto();
    }
}