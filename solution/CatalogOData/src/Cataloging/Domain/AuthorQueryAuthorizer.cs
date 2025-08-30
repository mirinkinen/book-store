using Common.Application.Authentication;
using Common.Domain;

namespace Cataloging.Domain;

public class AuthorQueryAuthorizer : IQueryAuthorizer<Author>
{
    private readonly IQueryAuthorizerRepository<Author> _queryAuthorizerRepository;

    public AuthorQueryAuthorizer(IQueryAuthorizerRepository<Author> queryAuthorizerRepository)
    {
        _queryAuthorizerRepository = queryAuthorizerRepository;
    }

    public IQueryable<Author> GetQuery(User user) =>
        _queryAuthorizerRepository.GetQuery()
            .Where(author => user.Organizations.Contains(author.OrganizationId));
}