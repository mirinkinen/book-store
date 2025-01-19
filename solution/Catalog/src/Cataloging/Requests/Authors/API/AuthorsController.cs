using Cataloging.Application;
using Cataloging.Requests.Authors.Application.AddAuthor;
using Cataloging.Requests.Authors.Application.DeleteAuthor;
using Cataloging.Requests.Authors.Application.GetAuthorById;
using Cataloging.Requests.Authors.Application.GetAuthors;
using Cataloging.Requests.Authors.Application.UpdateAuthor;
using Cataloging.Requests.Authors.Domain;
using Cataloging.Requests.Books.API;
using Cataloging.Requests.Books.Application.GetBooksFromAuthor;
using Cataloging.Requests.Books.Domain;
using Common.API;
using Common.Application;
using Common.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.Requests.Authors.API;

[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public class AuthorsController : ApiODataController
{
    private readonly IUserService _userService;

    public AuthorsController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("v1/authors")]
    [HttpGet("v1/authors/$count")]
    [EnableQuery(PageSize = 20)]
    [Produces<List<AuthorV1>>]
    public async Task<IQueryable<Author>> GetAuthors([FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetAuthorsQuery(_userService.GetUser(), queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query);

        return queryable.Query;
    }

    [HttpGet("v1/authors/{key}")]
    [EnableQuery]
    [Produces<AuthorV1>]
    public async Task<IActionResult> Get([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetAuthorByIdQuery(key, _userService.GetUser(), queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query);

        return Ok(SingleResult.Create(queryable.Query));
    }

    [HttpGet("v1/authors/{key}/books")]
    [EnableQuery(PageSize = 20)]
    [Produces<List<BookV1>>]
    public async Task<IQueryable<Book>> GetBooksFromAuthor([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBooksFromAuthorQuery(_userService.GetUser(), key, queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);

        return queryable.Query;
    }

    [HttpPost("v1/authors")]
    public Task<Author> Post([FromBody] AddAuthorDto addAuthorDto, [FromServices] IMessageBus bus)
    {
        var command = new AddAuthorCommand(addAuthorDto.Firstname, addAuthorDto.Lastname, addAuthorDto.Birthday,
            addAuthorDto.OrganizationId, _userService.GetUser());

        return bus.InvokeAsync<Author>(command);
    }

    [HttpPut("v1/authors/{key}")]
    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] AuthorPutDtoV1 dto, [FromServices] IMessageBus bus)
    {
        var command = new UpdateAuthorCommand(key, dto, _userService.GetUser());

        var author = await bus.InvokeAsync<Author?>(command);

        if (author == null)
        {
            return NotFound();
        }

        return Updated(author);

        return Ok();
    }

    [HttpPatch("v1/authors/{key}")]
    public async Task<IActionResult> Patch([FromRoute] Guid key, [FromBody] Delta<Author> delta,
        [FromServices] IMessageBus bus)
    {
        var command = new PatchAuthorCommand(key, delta, _userService.GetUser());

        var author = await bus.InvokeAsync<Author?>(command);

        if (author == null)
        {
            return NotFound();
        }

        return Updated(author);
    }

    [HttpDelete("v1/authors/{key}")]
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