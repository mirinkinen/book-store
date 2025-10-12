using Application.Common;
using Common.Domain;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthorById;

public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorNode>;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, AuthorNode>
{
    private readonly IAuthorReadRepository _readRepository;

    public GetAuthorByIdHandler(IAuthorReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<AuthorNode> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _readRepository.GetFirstOrDefaultAsync(request.Id, cancellationToken);

        if (author is null)
        {
            throw new EntityNotFoundException("Author not found", "author-not-found");
        }

        return author;
    }
}