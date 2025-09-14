using Application.Repositories;
using MediatR;

namespace Application.BookMutations.DeleteBook;

public class DeleteBookCommand : IRequest<DeleteBookOutput>
{
    public required Guid Id { get; set; }
}

public class DeleteBookOutput
{
    public required bool Success { get; set; }
    public required Guid Id { get; set; }
}

public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, DeleteBookOutput>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<DeleteBookOutput> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
    {
        var success = await _bookRepository.DeleteAsync(command.Id);
        
        return new DeleteBookOutput
        {
            Success = success,
            Id = command.Id
        };
    }
}
