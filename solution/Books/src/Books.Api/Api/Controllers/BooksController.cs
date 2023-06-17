using Books.Api.Application.Requests.GetBooks;
using Books.Api.Domain.Books;
using MediatR;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Books.Api.Api.Controllers;

public class BooksController : ODataController
{
    private readonly IMediator _mediatr;

    public BooksController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [EnableQuery]
    public Task<IQueryable<Book>> Get()
    {
        var getBooksQuery = new GetBooksQuery();
        return _mediatr.Send(getBooksQuery);
    }
}