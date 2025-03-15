using Cataloging.API.Models;
using Cataloging.Application;
using Cataloging.Application.GetBookById;
using Cataloging.Application.GetBooks;
using Cataloging.Domain;
using Common.API;
using Common.Application;
using Common.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.API;

[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public class BooksController : ApiODataController
{
    private readonly IUserAccessor _userAccessor;

    public BooksController(IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
    }

    [HttpGet("v1/books")]
    [HttpGet("v1/books/$count")]
    [EnableQuery(PageSize = 20)]
    [Produces<List<BookV1>>]
    public async Task<IQueryable<Book>> GetV1([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBooksQuery(queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return queryable.Query;
    }

    [HttpGet("v2/books")]
    [EnableQuery(PageSize = 20)]
    [Produces<List<BookV2>>]
    public async Task<IQueryable<Book>> GetV2([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBooksQuery(queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return queryable.Query;
    }

    [HttpGet("v3/books")]
    [EnableQuery(PageSize = 20)]
    [Produces<List<BookV3>>]
    public async Task<IQueryable<Book>> GetV3([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBooksQuery(queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return queryable.Query;
    }

    [HttpGet("v1/books/{key}")]
    [EnableQuery]
    [Produces<BookV1>]
    public async Task<IActionResult> GetV1([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBookByIdQuery(key, queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return Ok(SingleResult.Create(queryable.Query));
    }

    [HttpGet("v2/books/{key}")]
    [EnableQuery]
    [Produces<BookV2>]
    public async Task<IActionResult> GetV2([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBookByIdQuery(key, queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return Ok(SingleResult.Create(queryable.Query));
    }

    [HttpGet("v3/books/{key}")]
    [EnableQuery]
    [Produces<BookV3>]
    public async Task<IActionResult> GetV3([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBookByIdQuery(key, queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return Ok(SingleResult.Create(queryable.Query));
    }
}