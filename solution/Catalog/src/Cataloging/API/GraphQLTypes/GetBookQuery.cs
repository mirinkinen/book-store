using Cataloging.Requests.Books.Domain;
using HotChocolate.Types;

namespace Cataloging.API.GraphQLTypes;

[QueryType]
public static class GetBookQuery
{
    public static Book GetBook()
        => new Book(
            Guid.NewGuid(),
            "The Hobbit",
            new DateTime(1937, 9, 21),
            19.99m);
}