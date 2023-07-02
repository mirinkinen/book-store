using Cataloging.Application.Requests.Authors.AddAuthor;
using Cataloging.Application.Requests.Authors.DeleteAuthor;
using Cataloging.Application.Requests.Authors.GetAuthorById;
using Cataloging.Application.Requests.Authors.GetAuthors;
using Cataloging.Application.Requests.Authors.UpdateAuthor;
using Cataloging.Domain.Authors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Shared.Application.Authentication;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.Api.Authors;

[ODataRouteComponent("v1")]
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public partial class AuthorsController : ODataController
{
    private readonly IUserService _userService;

    public AuthorsController(IUserService userService)
    {
        _userService = userService;
    }

    [EnableQuery(PageSize = 20)]
    public Task<IQueryable<Author>> Get([FromServices] IMediator mediator)
    {
        var query = new GetAuthorsQuery(_userService.GetUser());
        return mediator.Send(query);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key, [FromServices] IMediator mediator)
    {
        var query = new GetAuthorByIdQuery(key, _userService.GetUser());
        var queryable = await mediator.Send(query);

        return Ok(SingleResult.Create(queryable));
    }

    public Task<Author> Post([FromBody] AddAuthorCommand addAuthorCommand, [FromServices] IMessageBus bus)
    {
        return bus.InvokeAsync<Author>(addAuthorCommand);
    }

    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateAuthorCommandDto dto, [FromServices] IMessageBus bus)
    {
        var command = new UpdateAuthorCommand(key, dto.Firstname, dto.Lastname, dto.Birthday, _userService.GetUser());

        var author = await bus.InvokeAsync<Author?>(command);

        if (author == null)
        {
            return NotFound();
        }

        return Updated(author);
    }

    public async Task<IActionResult> Delete([FromRoute] Guid key, [FromServices] IMessageBus bus)
    {
        var command = new DeleteAuthorCommand(key);

        var author = await bus.InvokeAsync<Author>(command);

        if (author == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}