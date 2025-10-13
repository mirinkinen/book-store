using Application.AuthorQueries;
using Domain.Authors;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Repositories;

public class AuthorReadRepository : ReadRepository<Author, AuthorNode>, IAuthorReadRepository
{
    public AuthorReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    protected override Expression<Func<Author, AuthorNode>> GetProjection()
    {
        return AuthorExtensions.ProjectToNode();
    }

    protected override Func<SortDefinition<AuthorNode>, SortDefinition<AuthorNode>> GetDefaultOrder()
    {
        return sort => sort.IfEmpty(o => o.AddAscending(a => a.FirstName).AddAscending(a => a.LastName))
            .AddDescending(t => t.Id);
    }
}