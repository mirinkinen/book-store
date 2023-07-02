using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using MediatR;
using Shared.Application.Auditing;
using Shared.Application.Authentication;

namespace Cataloging.Application.Requests.Books.GetBooks;

public record GetBooksQuery(User Actor) : IAuditableQuery<IQueryable<Book>>;

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IQueryable<Book>>
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetBooksHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Book>());
    }
}