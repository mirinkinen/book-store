using Application.Repositories;
using Domain;
using MediatR;

namespace Application.AuthorMutations.UpdateAuthor;

public class UpdateAuthorInput : IRequest<AuthorUpdatedOutput>
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
}

public class AuthorUpdatedOutput
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
}

public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorInput, AuthorUpdatedOutput>
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<AuthorUpdatedOutput> Handle(UpdateAuthorInput input, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(input.Id);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {input.Id} not found");
        }
        
        author.Update(input.FirstName, input.LastName, input.Birthdate);
        var updatedAuthor = await _authorRepository.UpdateAsync(author);
        
        return new AuthorUpdatedOutput
        {
            Id = updatedAuthor.Id,
            FirstName = updatedAuthor.FirstName,
            LastName = updatedAuthor.LastName,
            Birthdate = updatedAuthor.Birthdate
        };
    }
}
