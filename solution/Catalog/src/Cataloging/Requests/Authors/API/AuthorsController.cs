using Cataloging.Application;
using Cataloging.Requests.Authors.Application;
using Cataloging.Requests.Authors.Application.AddAuthor;
using Cataloging.Requests.Authors.Application.DeleteAuthor;
using Cataloging.Requests.Authors.Application.GetAuthorById;
using Cataloging.Requests.Authors.Application.GetAuthors;
using Cataloging.Requests.Authors.Application.UpdateAuthor;
using Cataloging.Requests.Authors.Domain;
using Cataloging.Requests.Books.Application.GetBooksFromAuthor;
using Cataloging.Requests.Books.Domain;
using Common.Application;
using Common.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.Requests.Authors.API;

[ODataRouteComponent("v1")]
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public class AuthorsController : ODataController
{
    private readonly IUserService _userService;

    public AuthorsController(IUserService userService)
    {
        _userService = userService;
    }

    [EnableQuery(PageSize = 20)]
    public async Task<IQueryable<Author>> Get([FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetAuthorsQuery(_userService.GetUser(), queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query);

        return queryable.Query;
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetAuthorByIdQuery(key, _userService.GetUser(), queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query);

        return Ok(SingleResult.Create(queryable.Query));
    }

    [EnableQuery(PageSize = 20)]
    public async Task<IQueryable<Book>> GetBooksFromAuthor([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBooksFromAuthorQuery(_userService.GetUser(), key, queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return queryable.Query;
    }

    public Task<Author> Post([FromBody] AddAuthorDto addAuthorDto, [FromServices] IMessageBus bus)
    {
        var command = new AddAuthorCommand(addAuthorDto.Firstname, addAuthorDto.Lastname, addAuthorDto.Birthday,
            addAuthorDto.OrganizationId, _userService.GetUser());

        return bus.InvokeAsync<Author>(command);
    }

    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateAuthorDto dto,
        [FromServices] IMessageBus bus)
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
        var command = new DeleteAuthorCommand(key, _userService.GetUser());

        var author = await bus.InvokeAsync<Author?>(command);

        if (author == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}