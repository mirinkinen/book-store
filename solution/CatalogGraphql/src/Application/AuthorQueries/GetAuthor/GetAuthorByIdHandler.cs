using Application.Repositories;
using Domain;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.AuthorQueries.GetAuthor;

public class GetAuthorByIdInput : IRequest<GetAuthorOutput?>
{
    [SetsRequiredMembers]
    public GetAuthorByIdInput(Guid id)
    {
        Id = id;
    }   
    
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

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdInput, GetAuthorOutput?>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorByIdHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<GetAuthorOutput?> Handle(GetAuthorByIdInput request, CancellationToken cancellationToken)
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
