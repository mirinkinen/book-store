using Cataloging.Application.Requests.Authors.AddAuthor;
using Cataloging.Application.Requests.Authors.DeleteAuthor;
using Cataloging.Application.Requests.Authors.GetAuthorById;
using Cataloging.Application.Requests.Authors.GetAuthors;
using Cataloging.Application.Requests.Authors.UpdateAuthor;
using Cataloging.Domain.Authors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Shared.Application;
using Shared.Application.Authentication;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.Api.Authors;

[ODataRouteComponent("v1")]
public partial class AuthorsController : ODataController
{
    private readonly IMessageBus _bus;
    private readonly IUserService _userService;

    public AuthorsController(IMessageBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [EnableQuery(PageSize = 20)]
    public async Task<IQueryable<Author>> Get()
    {
        var query = new GetAuthorsQuery(_userService.GetUser());
        var queryable = await _bus.InvokeAsync<QueryableResponse<Author>>(query);

        return queryable.Query;
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key)
    {
        var query = new GetAuthorByIdQuery(key, _userService.GetUser());
        var queryable = await _bus.InvokeAsync<QueryableResponse<Author>>(query);

        return Ok(SingleResult.Create(queryable.Query));
    }

    public Task<Author> Post([FromBody] AddAuthorCommand addAuthorCommand)
    {
        return _bus.InvokeAsync<Author>(addAuthorCommand);
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Dto is never null.")]
    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateAuthorCommandDto dto)
    {
        var command = new UpdateAuthorCommand(key, dto.Firstname, dto.Lastname, dto.Birthday, _userService.GetUser());

        var author = await _bus.InvokeAsync<Author?>(command);

        if (author == null)
        {
            return NotFound();
        }

        return Updated(author);
    }

    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        var command = new DeleteAuthorCommand(key);

        var author = await _bus.InvokeAsync<Author>(command);

        if (author == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}