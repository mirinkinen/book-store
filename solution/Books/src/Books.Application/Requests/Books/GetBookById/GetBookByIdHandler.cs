using Books.Application.Services;
using Books.Domain.Books;
using MediatR;

namespace Books.Application.Requests.Books.GetBookById;

public record GetBookByIdQuery(Guid BookId) : IRequest<IQueryable<Book>>;

public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, IQueryable<Book>>
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetBookByIdHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Book>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Book>().Where(a => a.Id == request.BookId));
    }
}