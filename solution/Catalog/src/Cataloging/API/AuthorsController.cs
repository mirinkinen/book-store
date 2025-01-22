using Cataloging.API.Models;
using Cataloging.Application;
using Cataloging.Application.AddAuthor;
using Cataloging.Application.DeleteAuthor;
using Cataloging.Application.GetAuthorById;
using Cataloging.Application.GetAuthors;
using Cataloging.Application.GetBooksFromAuthor;
using Cataloging.Application.UpdateAuthor;
using Cataloging.Domain;
using Common.API;
using Common.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.API;

[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public class AuthorsController : ApiODataController
{
    [HttpGet("v1/authors")]
    [HttpGet("v1/authors/$count")]
    [EnableQuery(PageSize = 20)]
    [Produces<List<AuthorV1>>]
    public async Task<IQueryable<Author>> GetAuthors([FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetAuthorsQuery(queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query);
    
        return queryable.Query;
    }

    [HttpGet("v1/authors/{key}")]
    [EnableQuery]
    [Produces<AuthorV1>]
    public async Task<IActionResult> Get([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetAuthorByIdQuery(key, queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Author>>(query);
    
        return Ok(SingleResult.Create(queryable.Query));
    }
    
    [HttpGet("v1/authors/{key}/books")]
    [EnableQuery(PageSize = 20)]
    [Produces<List<BookV1>>]
    public async Task<IQueryable<Book>> GetBooksFromAuthor([FromRoute] Guid key, [FromServices] IMessageBus bus,
        [FromServices] IQueryAuthorizer queryAuthorizer)
    {
        var query = new GetBooksFromAuthorQuery(key, queryAuthorizer);
        var queryable = await bus.InvokeAsync<QueryableResponse<Book>>(query);
    
        return queryable.Query;
    }
    
    [HttpPost("v1/authors")]
    [Produces<AuthorV1>]
    public Task<Author> Post([FromBody] PostAuthorDtoV1 postAuthorDtoV1, [FromServices] IMessageBus bus)
    {
        var command = new PostAuthorCommand(postAuthorDtoV1.FirstName, postAuthorDtoV1.LastName, postAuthorDtoV1.Birthday,
            postAuthorDtoV1.OrganizationId);
    
        return bus.InvokeAsync<Author>(command);
    }
    
    [HttpPut("v1/authors/{key}")]
    [Produces<AuthorV1>]
    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] PutAuthorDtoV1 dto, [FromServices] IMessageBus bus)
    {
        var command = new UpdateAuthorCommand(key, dto.Birthday, dto.FirstName, dto.LastName);
    
        var author = await bus.InvokeAsync<Author?>(command);
    
        if (author == null)
        {
            return NotFound();
        }
    
        return Updated(author);
    }
    
    [HttpPatch("v1/authors/{key}")]
    [Produces<AuthorV1>]
    public async Task<IActionResult> Patch([FromRoute] Guid key, [FromBody] Delta<Author> delta,
        [FromServices] IMessageBus bus)
    {
        var command = new PatchAuthorCommand(key, delta);
    
        var author = await bus.InvokeAsync<Author?>(command);
    
        if (author == null)
        {
            return NotFound();
        }
    
        return Ok();
    }
    
    [HttpDelete("v1/authors/{key}")]
    public async Task<IActionResult> Delete([FromRoute] Guid key, [FromServices] IMessageBus bus)
    {
        var command = new DeleteAuthorCommand(key);
    
        var author = await bus.InvokeAsync<Author?>(command);
    
        if (author == null)
        {
            return NotFound();
        }
    
        return NoContent();
    }
}