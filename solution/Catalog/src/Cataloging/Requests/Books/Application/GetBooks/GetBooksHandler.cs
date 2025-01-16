using Cataloging.Application;
using Cataloging.Requests.Books.Domain;
using Common.Api.Application;
using Common.Api.Application.Authentication;
using System.Diagnostics;

namespace Cataloging.Requests.Books.Application.GetBooks;

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