using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using Common.Application;
using Common.Application.Authentication;
using System.Diagnostics;

namespace Cataloging.Application.Requests.Books.GetBooks;

public record GetBooksQuery(User Actor, IQueryAuthorizer QueryAuthorizer);

public static class GetBooksHandler
{
    private static readonly ActivitySource _activitySource = new(nameof(GetBooksQuery));
    
    public static QueryableResponse<Book> Handle(GetBooksQuery request)
    {
        using var activity = _activitySource.StartActivity();

        activity.SetTag("TestKey", "TestValue");
        
        return new QueryableResponse<Book>(
            request.QueryAuthorizer.GetAuthorizedEntities<Book>());
    }
}