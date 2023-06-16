using Books.Api.Domain.Books;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    public BooksController()
    { }

    [HttpGet]
    public IEnumerable<Book> GetBooks()
    {
        return new List<Book> {
            new Book("Hello world", DateTime.UtcNow, Guid.NewGuid()),
            new Book("Hello world", DateTime.UtcNow, Guid.NewGuid())
        };
    }
}