using Common.Application.Authentication;
using Common.Domain;

namespace Cataloging.Domain;

public class BookQueryAuthorizerAuthorizer : IQueryAuthorizer<Book>
{
    private readonly IQueryAuthorizerRepository _queryAuthorizerRepository;

    public BookQueryAuthorizerAuthorizer(IQueryAuthorizerRepository queryAuthorizerRepository)
    {
        _queryAuthorizerRepository = queryAuthorizerRepository;
    }

    public IQueryable<Book> GetQuery(User user) =>
        _queryAuthorizerRepository.GetBookQuery()
            .Where(book => user.Organizations.Contains(book.Author.OrganizationId));
}