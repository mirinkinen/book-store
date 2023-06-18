using Books.Application.Requests.Authors.GetAuthorById;
using Books.Application.Requests.Authors.GetAuthors;
using Books.Domain.Authors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Books.Api.Controllers;

public class AuthorsController : ODataController
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
}