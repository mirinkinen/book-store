using Books.Application.Requests.Authors.AddAuthor;
using Books.Application.Requests.Authors.GetAuthorById;
using Books.Application.Requests.Authors.GetAuthors;
using Books.Domain.Authors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Diagnostics.CodeAnalysis;

namespace Books.Api.Controllers;

public partial class AuthorsController : ODataController
{
    private readonly IMediator _mediatr;

    public AuthorsController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [EnableQuery]
    public Task<IQueryable<Author>> Get()
    {
        var query = new GetAuthorsQuery();
        return _mediatr.Send(query);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key)
    {
        var query = new GetAuthorByIdQuery(key);
        var author = await _mediatr.Send(query);

        if (author == null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    public Task<Author> Post([FromBody] AddAuthorCommand addAuthorCommand)
    {
        return _mediatr.Send(addAuthorCommand);
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Dto is never null.")]
    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateAuthorCommandDto dto)
    {
        var command = new UpdateAuthorCommand(key, dto.Firstname, dto.Lastname, dto.Birthday);

        var author = await _mediatr.Send(command);

        if (author == null)
        {
            return NotFound();
        }

        return Updated(author);
    }

    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        var command = new DeleteAuthorCommand(key);

        var author = await _mediatr.Send(command);

        if (author == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}