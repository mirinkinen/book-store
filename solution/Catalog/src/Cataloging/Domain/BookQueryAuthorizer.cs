using Common.Application.Authentication;
using Common.Domain;

namespace Cataloging.Domain;

public class BookQueryAuthorizer : IQueryAuthorizer<Book>
{
    private readonly IQueryAuthorizerRepository<Book> _queryAuthorizerRepository;

    public BookQueryAuthorizer(IQueryAuthorizerRepository<Book> queryAuthorizerRepository)
    {
        _queryAuthorizerRepository = queryAuthorizerRepository;
    }

    public IQueryable<Book> GetQuery(User user) =>
        _queryAuthorizerRepository.GetQuery()
            .Where(book => user.Organizations.Contains(book.Author.OrganizationId));
}