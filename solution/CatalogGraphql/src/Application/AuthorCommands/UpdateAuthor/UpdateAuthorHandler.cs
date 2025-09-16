using Application.Types;
using Domain;
using MediatR;

namespace Application.AuthorCommands.UpdateAuthor;

public record UpdateAuthorCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime Birthdate) : IRequest<AuthorDto>;

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
