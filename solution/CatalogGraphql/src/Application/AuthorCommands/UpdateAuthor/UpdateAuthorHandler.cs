using Application.Types;
using Domain;
using MediatR;

namespace Application.AuthorCommands.UpdateAuthor;

public class UpdateAuthorCommand : IRequest<AuthorDto>
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
}

public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto>
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<AuthorDto> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(command.Id);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {command.Id} not found");
        }
        
        author.Update(command.FirstName, command.LastName, command.Birthdate);
        
        var updatedAuthor = await _authorRepository.UpdateAsync(author);

        return updatedAuthor.ToDto();
        
    }
}
