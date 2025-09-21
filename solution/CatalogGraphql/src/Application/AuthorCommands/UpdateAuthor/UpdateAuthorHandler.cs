using Domain;
using MediatR;

namespace Application.AuthorCommands.UpdateAuthor;

public record UpdateAuthorCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly Birthdate) : IRequest<Author>;

public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, Author>
{
    private readonly IAuthorWriteRepository _authorWriteRepository;

    public UpdateAuthorHandler(IAuthorWriteRepository authorWriteRepository)
    {
        _authorWriteRepository = authorWriteRepository;
    }

    public async Task<Author> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = await _authorWriteRepository.FirstOrDefaultAsync(command.Id);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {command.Id} not found");
        }

        author.Update(command.FirstName, command.LastName, command.Birthdate);

        await _authorWriteRepository.SaveChangesAsync(cancellationToken);

        return author;
    }
}