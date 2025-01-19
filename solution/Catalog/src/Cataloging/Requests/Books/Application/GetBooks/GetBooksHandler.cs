using Cataloging.Application;
using Cataloging.Requests.Books.Domain;
using Common.Application;
using Common.Application.Authentication;
using System.Diagnostics;

namespace Cataloging.Requests.Books.Application.GetBooks;

public record GetBooksQuery(User Actor, IQueryAuthorizer QueryAuthorizer);

public static class GetBooksHandler
{
    private static readonly ActivitySource _activitySource = new(nameof(GetBooksQuery));
    
    public static async Task<QueryableResponse<Book>> Handle(GetBooksQuery request)
    {
        using var activity = _activitySource.StartActivity();

        activity.SetTag("TestKey", "TestValue");
        
        return new QueryableResponse<Book>(
            await request.QueryAuthorizer.GetAuthorizedEntities<Book>());
    }
}