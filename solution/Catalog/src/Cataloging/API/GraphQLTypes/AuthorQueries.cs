using Cataloging.Application;
using Cataloging.Domain;
using Common.Application;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Cataloging.API.GraphQLTypes;

[QueryType]
public static class AuthorQueries
{
    public static async Task<IQueryable<Author>> GetAuthors([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer
        queryAuthorizer, CancellationToken cancellationToken)
    {
        var query = new Application.GetAuthors.GetAuthorsQuery(queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query, cancellationToken);

        return queryable.Query;
    }
}

[QueryType]
public static class Author2Queries
{
    public static async Task<IQueryable<Author>> GetAuthors([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer
        queryAuthorizer, CancellationToken cancellationToken)
    {
        var query = new Application.GetAuthors.GetAuthorsQuery(queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query, cancellationToken);

        return queryable.Query;
    }
}