using Cataloging.Application.Requests.Books.GetBookById;
using Cataloging.Application.Requests.Books.GetBooks;
using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using Common.Application;
using Common.Application.Auditing;
using Common.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.Api.Books;

[ODataRouteComponent("v1")]
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public class BooksController : ODataController
{
    private readonly IUserService _userService;

    public BooksController(IUserService userService)
    {
        _userService = userService;
    }

    [EnableQuery(PageSize = 20)]
    public async Task<IQueryable<Book>> Get([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer queryAuthorizer,
        [FromServices] IAuditContext auditContext)
    {
        var query = new GetBooksQuery(_userService.GetUser(), queryAuthorizer, auditContext);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return queryable.Query;
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key, [FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBookByIdQuery(key, queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return Ok(SingleResult.Create(queryable.Query));
    }
}