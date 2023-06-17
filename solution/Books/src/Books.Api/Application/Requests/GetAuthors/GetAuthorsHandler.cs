using Books.Api.Domain.Authors;
using Books.Api.Infrastructure.Database;
using MediatR;

namespace Books.Api.Application.Requests.GetAuthors;

public class GetAuthorsQuery : IRequest<IQueryable<Author>>
{
}

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorsQuery, IQueryable<Author>>
{
    private readonly BooksDbContext _booksDbContext;

    public GetAuthorByIdHandler(BooksDbContext booksDbContext)
    {
        _booksDbContext = booksDbContext;
    }

    public Task<IQueryable<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_booksDbContext.Authors.AsQueryable());
    }
}