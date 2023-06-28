using Cataloging.Application.Requests.Authors.AddAuthor;
using Cataloging.Application.Requests.Authors.DeleteAuthor;
using Cataloging.Application.Requests.Authors.GetAuthorById;
using Cataloging.Application.Requests.Authors.GetAuthors;
using Cataloging.Application.Requests.Authors.UpdateAuthor;
using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Api.Authors;

[ODataRouteComponent("v1")]
public partial class AuthorsController : ODataController
{
    private readonly IMediator _mediatr;
    private readonly IUserService _userService;

    public AuthorsController(IMediator mediatr, IUserService userService)
    {
        _mediatr = mediatr;
        _userService = userService;
    }

    [EnableQuery(PageSize = 20)]
    public Task<IQueryable<Author>> Get()
    {
        var query = new GetAuthorsQuery(_userService.GetUser());
        return _mediatr.Send(query);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key)
    {
        var query = new GetAuthorByIdQuery(key, _userService.GetUser());
        var authorQuery = await _mediatr.Send(query);

        return Ok(SingleResult.Create(authorQuery));
    }

    public Task<Author> Post([FromBody] AddAuthorCommand addAuthorCommand)
    {
        return _mediatr.Send(addAuthorCommand);
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Dto is never null.")]
    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateAuthorCommandDto dto)
    {
        var command = new UpdateAuthorCommand(key, dto.Firstname, dto.Lastname, dto.Birthday, _userService.GetUser());

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