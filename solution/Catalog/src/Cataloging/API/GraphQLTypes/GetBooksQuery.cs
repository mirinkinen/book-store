using Cataloging.Application;
using Cataloging.Application.GetBooks;
using Cataloging.Domain;
using Common.Application;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Cataloging.API.GraphQLTypes;

[QueryType]
public static class GetBooksQuery
{
    public static async Task<IQueryable<Book>> GetBooks([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new Application.GetBooks.GetBooksQuery(queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return queryable.Query;
    }
}