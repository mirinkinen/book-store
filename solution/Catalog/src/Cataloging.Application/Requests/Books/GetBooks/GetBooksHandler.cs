using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using Shared.Application;
using Shared.Application.Auditing;
using Shared.Application.Authentication;

namespace Cataloging.Application.Requests.Books.GetBooks;

public record GetBooksQuery(User Actor) : IAuditableQuery;

public class GetBooksHandler
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetBooksHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public QueryableResponse<Book> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return new QueryableResponse<Book>(_queryAuthorizer.GetAuthorizedEntities<Book>());
    }
}