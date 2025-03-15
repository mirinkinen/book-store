using Common.Application.Authentication;
using Common.Domain;

namespace Cataloging.Domain;

public class AuthorQueryAuthorizerAuthorizer : IQueryAuthorizer<Author>
{
    private readonly IQueryAuthorizerRepository _queryAuthorizerRepository;

    public AuthorQueryAuthorizerAuthorizer(IQueryAuthorizerRepository queryAuthorizerRepository)
    {
        _queryAuthorizerRepository = queryAuthorizerRepository;
    }

    public IQueryable<Author> GetQuery(User user) =>
        _queryAuthorizerRepository.GetAuthorQuery()
            .Where(author => user.Organizations.Contains(author.OrganizationId));
}