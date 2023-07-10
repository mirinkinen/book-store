using Cataloging.Domain.Authors;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace Cataloging.Application.Requests.Authors;

public static class LoadAuthorMiddleware
{
    public static async Task<(HandlerContinuation, Author?)> LoadAsync(IAuthorCommand command, ILogger logger,
        IAuthorRepository authorRepository, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetAuthorById(command.AuthorId, cancellationToken);
        
        return (author == null ? HandlerContinuation.Stop : HandlerContinuation.Continue, author);
    }
}