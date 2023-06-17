using Books.Api.Domain.Books;
using Books.Api.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Books.Api.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ODataController
{
    public BooksController()
    { }

    [HttpGet]
    [EnableQuery]
    public IQueryable<Book> GetBooks([FromServices] BooksDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        return dbContext.Books.AsQueryable().OrderBy(b => b.Title);
    }
}