using Books.Api.Domain.Authors;
using Books.Api.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    public AuthorsController()
    { }

    [HttpGet]
    public IQueryable<Author> GetAuthors([FromServices] BooksDbContext dbContext)
    {
        return dbContext.Authors.AsQueryable();
    }
}