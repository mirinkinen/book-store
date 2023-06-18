using Books.Domain.Books;
using MediatR;

namespace Books.Application.Requests.Books.GetBooks;

public class GetBooksQuery : IRequest<IQueryable<Book>>
{
}

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