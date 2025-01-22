using Cataloging.Application;
using Cataloging.Requests.Books.Domain;
using Common.Application;
using System.Diagnostics;

namespace Cataloging.Requests.Books.Application.GetBooks;

public record GetBooksQuery(IQueryAuthorizer QueryAuthorizer);

public static class GetBooksHandler
{
    private static readonly ActivitySource _activitySource = new(nameof(GetBooksQuery));

    public static async Task<QueryableResponse<Book>> Handle(GetBooksQuery request)
    {
        using var activity = _activitySource.StartActivity();
        activity?.SetTag("TestKey", "TestValue");

        var query = await request.QueryAuthorizer.GetAuthorizedEntities<Book>();
        return new QueryableResponse<Book>(query);
    }
}