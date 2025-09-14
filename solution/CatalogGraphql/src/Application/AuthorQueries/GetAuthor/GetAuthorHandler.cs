using Application.Repositories;
using Domain;
using MediatR;

namespace Application.AuthorQueries.GetAuthor;

public class GetAuthorInput : IRequest<GetAuthorOutput?>
{
    public required Guid Id { get; set; }
}

public class GetAuthorOutput
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
    public required Guid OrganizationId { get; set; }
}

public class GetAuthorHandler : IRequestHandler<GetAuthorInput, GetAuthorOutput?>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<GetAuthorOutput?> Handle(GetAuthorInput request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(request.Id);
        
        if (author == null)
            return null;
            
        return new GetAuthorOutput
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Birthdate = author.Birthdate,
            OrganizationId = author.OrganizationId
        };
    }
}
