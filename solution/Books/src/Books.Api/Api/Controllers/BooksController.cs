using Books.Api.Application.Requests.GetBooks;
using Books.Api.Domain.Books;
using Books.Api.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Books.Api.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ODataController
{
    private readonly IMediator _mediatr;

    public BooksController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    [EnableQuery]
    public Task<IQueryable<Book>> GetBooks()
    {
        var getBooksQuery = new GetBooksQuery();
        return _mediatr.Send(getBooksQuery);
    }
}