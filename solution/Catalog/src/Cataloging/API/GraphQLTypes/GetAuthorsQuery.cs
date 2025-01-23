using Cataloging.Application;
using Cataloging.Application.GetBooks;
using Cataloging.Domain;
using Common.Application;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Cataloging.API.GraphQLTypes;

[QueryType]
public static class GetAuthorsQuery
{
    public static async Task<IQueryable<Author>> GetAuthors([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer 
            queryAuthorizer, CancellationToken cancellationToken)
    {
        var query = new Application.GetAuthors.GetAuthorsQuery(queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query);

        return queryable.Query;
    }
}