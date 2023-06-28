using Books.Application.Auditing;
using Books.Application.Services;
using Books.Domain.Books;
using MediatR;

namespace Books.Application.Requests.Books.GetBooks;

public record GetBooksQuery(User Actor) : IAuditRequest<IQueryable<Book>>
{
    public OperationType OperationType => OperationType.Read;
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