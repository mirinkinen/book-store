using Application3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookUseCases _bookController;

        public BooksController(IBookUseCases bookController)
        {
            _bookController = bookController;
        }

        // GET: api/books
        [HttpGet]
        public IEnumerable<BookDto> Get()
        {
            var books = _bookController.GetBooks();

            return books.Select(book => new BookDto
            {
                AuthorId = book.AuthorId,
                DateOfPublication = book.DateOfPublication,
                Genre = book.Genre.ToString(),
                Id = book.Id,
                Name = book.Name
            });
        }

        // POST api/books
        [HttpPost]
        public IActionResult Post([FromBody] BookCreationDto bookCreationDto)
        {
            _bookController.AddBook(bookCreationDto);

            return Ok();
        }
    }
}