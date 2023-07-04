using Cataloging.Application.Requests.Authors.AddAuthor;
using Cataloging.Application.Requests.Authors.DeleteAuthor;
using Cataloging.Application.Requests.Authors.GetAuthorById;
using Cataloging.Application.Requests.Authors.GetAuthors;
using Cataloging.Application.Requests.Authors.UpdateAuthor;
using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
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
    public async Task<IQueryable<Author>> Get([FromServices] IMessageBus bus, [FromServices] IQueryAuthorizer queryAuthorizer,
        [FromServices] IAuditContext auditContext)
    {
        var query = new GetAuthorsQuery(_userService.GetUser(), queryAuthorizer, auditContext);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query);

        return queryable.Query;
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key, [FromServices] IMessageBus bus, 
        [FromServices] IQueryAuthorizer queryAuthorizer, [FromServices] IAuditContext auditContext)
    {
        var query = new GetAuthorByIdQuery(key, _userService.GetUser(), queryAuthorizer, auditContext);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query);

        return Ok(SingleResult.Create(queryable.Query));
    }

    public Task<Author> Post([FromBody] AddAuthorDto addAuthorDto, [FromServices] IMessageBus bus)
    {
        var command = new AddAuthorCommand(addAuthorDto.Firstname, addAuthorDto.Lastname, addAuthorDto.Birthday,
            addAuthorDto.OrganizationId, _userService.GetUser());

        return bus.InvokeAsync<Author>(command);
    }

    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateAuthorDto dto, [FromServices] IMessageBus bus)
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