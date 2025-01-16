using Cataloging.Requests.Authors.Domain;
using Wolverine;

namespace Cataloging.Requests.Authors.Application.Middleware;

public static class LoadAuthorMiddleware
{
    public static async Task<(HandlerContinuation, Author?)> LoadAsync(IAuthorCommand command, ILogger logger,
        IAuthorRepository authorRepository, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetAuthorById(command.AuthorId, cancellationToken);

        return (author == null ? HandlerContinuation.Stop : HandlerContinuation.Continue, author);
    }
}