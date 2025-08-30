using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;
using Common.Domain;
using System.Diagnostics;

namespace Cataloging.Application.GetBooks;

public record GetBooksQuery(IQueryAuthorizer<Book> QueryAuthorizer);

public static class GetBooksHandler
{
    private static readonly ActivitySource _activitySource = new(nameof(GetBooksQuery));

    public static async Task<QueryableResponse<Book>> Handle(GetBooksQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        using var activity = _activitySource.StartActivity();
        activity?.SetTag("TestKey", "TestValue");

        var query = request.QueryAuthorizer.GetQuery(user);
        return new QueryableResponse<Book>(query);
    }
}