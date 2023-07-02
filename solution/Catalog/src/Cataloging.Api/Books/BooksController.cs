using Cataloging.Application.Requests.Books.GetBookById;
using Cataloging.Application.Requests.Books.GetBooks;
using Cataloging.Domain.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Common.Application.Authentication;
using System.Diagnostics.CodeAnalysis;

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
    public Task<IQueryable<Book>> Get([FromServices] IMediator mediator)
    {
        var query = new GetBooksQuery(_userService.GetUser());
        return mediator.Send(query);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key, [FromServices] IMediator mediator)
    {
        var query = new GetBookByIdQuery(key);
        var bookQuery = await mediator.Send(query);

        return Ok(SingleResult.Create(bookQuery));
    }
}