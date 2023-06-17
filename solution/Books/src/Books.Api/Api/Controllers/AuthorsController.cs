using Books.Api.Application.Requests.GetAuthors;
using Books.Api.Domain.Authors;
using MediatR;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Books.Api.Api.Controllers;

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
        var getAuthorsQuery = new GetAuthorsQuery();
        return _mediatr.Send(getAuthorsQuery);
    }
}