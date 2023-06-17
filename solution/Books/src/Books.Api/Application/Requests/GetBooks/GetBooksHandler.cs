using Books.Api.Domain.Books;
using MediatR;

namespace Books.Api.Application.Requests.GetBooks;

public class GetBooksQuery : IRequest<IQueryable<Book>>
{
}

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IQueryable<Book>>
{
    private readonly QueryAuthorizer _queryAuthorizer;

    public GetBooksHandler(QueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Book>());
    }
}