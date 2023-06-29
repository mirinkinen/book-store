using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using Shared.Application;

namespace Cataloging.Application.Requests.Books.GetBookById;

public record GetBookByIdQuery(Guid BookId);

public class GetBookByIdHandler
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetBookByIdHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public QueryableResponse<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new QueryableResponse<Book>(_queryAuthorizer.GetAuthorizedEntities<Book>().Where(a => a.Id == request.BookId));
    }
}