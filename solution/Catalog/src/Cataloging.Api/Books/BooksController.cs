using Cataloging.Application.Requests.Books.GetBookById;
using Cataloging.Application.Requests.Books.GetBooks;
using Cataloging.Domain.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Shared.Application;
using Shared.Application.Authentication;
using Wolverine;

namespace Cataloging.Api.Books;

[ODataRouteComponent("v1")]
public class BooksController : ODataController
{
    private readonly IMessageBus _bus;
    private readonly IUserService _userService;

    public BooksController(IMessageBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [EnableQuery(PageSize = 20)]
    public async Task<IQueryable<Book>> Get()
    {
        var query = new GetBooksQuery(_userService.GetUser());
        var data = await _bus.InvokeAsync<QueryableResponse<Book>>(query);
        return data.Query;
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key)
    {
        var query = new GetBookByIdQuery(key);
        var bookQuery = await _bus.InvokeAsync<QueryableResponse<Book>>(query);

        return Ok(SingleResult.Create(bookQuery.Query));
    }
}