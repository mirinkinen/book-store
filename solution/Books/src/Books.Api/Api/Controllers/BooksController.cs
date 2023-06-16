using Books.Api.Domain.Books;
using Books.Api.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    public BooksController()
    { }

    [HttpGet]
    public IEnumerable<Book> GetBooks([FromServices] BooksDbContext dbContext)
    {
        return dbContext.Books.AsQueryable().OrderBy(b => b.Title);
    }
}