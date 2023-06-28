using Cataloging.Application.Requests.Books.GetBookById;
using Cataloging.Application.Requests.Books.GetBooks;
using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Cataloging.Api.Books;

[ODataRouteComponent("v1")]
public class BooksController : ODataController
{
    private readonly IMediator _mediatr;
    private readonly IUserService _userService;

    public BooksController(IMediator mediatr, IUserService userService)
    {
        _mediatr = mediatr;
        _userService = userService;
    }

    [EnableQuery(PageSize = 20)]
    public Task<IQueryable<Book>> Get()
    {
        var query = new GetBooksQuery(_userService.GetUser());
        return _mediatr.Send(query);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key)
    {
        var query = new GetBookByIdQuery(key);
        var bookQuery = await _mediatr.Send(query);

        return Ok(SingleResult.Create(bookQuery));
    }
}