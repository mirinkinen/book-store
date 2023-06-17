using Books.Api.Domain.Books;
using MediatR;
using Microsoft.AspNetCore.OData.Results;

namespace Books.Api.Application.Requests.GetBookById;

public record GetBookByIdQuery(Guid BookId) : IRequest<SingleResult<Book>>;

public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, SingleResult<Book>>
{
    private readonly QueryAuthorizer _queryAuthorizer;

    public GetBookByIdHandler(QueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<SingleResult<Book>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var query = _queryAuthorizer.GetAuthorizedEntities<Book>()
            .Where(a => a.Id == request.BookId);

        return Task.FromResult(SingleResult.Create(query));
    }
}