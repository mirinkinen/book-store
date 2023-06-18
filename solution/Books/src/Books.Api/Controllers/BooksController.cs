using Books.Application.Requests.Books.GetBookById;
using Books.Application.Requests.Books.GetBooks;
using Books.Domain.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Books.Api.Controllers;

public class BooksController : ODataController
{
    private readonly IMediator _mediatr;

    public BooksController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [EnableQuery(PageSize = 20)]
    public Task<IQueryable<Book>> Get()
    {
        var query = new GetBooksQuery();
        return _mediatr.Send(query);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key)
    {
        var query = new GetBookByIdQuery(key);
        var book = await _mediatr.Send(query);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }
}