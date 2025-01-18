using Cataloging.Application;
using Cataloging.Requests.Books.Application.GetBookById;
using Cataloging.Requests.Books.Application.GetBooks;
using Cataloging.Requests.Books.Domain;
using Common.API;
using Common.Application;
using Common.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.Requests.Books.API;

[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public class BooksController : ApiODataController
{
    private readonly IUserService _userService;

    public BooksController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("v1/books")]
    [HttpGet("v2/books")]
    [HttpGet("v1/books/$count")]
    [EnableQuery(PageSize = 20)]
    public async Task<IQueryable<Book>> Get([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBooksQuery(_userService.GetUser(), queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return queryable.Query;
    }

    [HttpGet("v1/books/{key}")]
    [HttpGet("v2/books/{key}")]
    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBookByIdQuery(key, queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return Ok(SingleResult.Create(queryable.Query));
    }
}