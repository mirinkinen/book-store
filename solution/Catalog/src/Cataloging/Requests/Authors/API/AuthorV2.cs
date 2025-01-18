using Cataloging.Requests.Books.API;
using GraphQLParser.AST;

namespace Cataloging.Requests.Authors.API;

public class AuthorV2
{
    public Guid Id { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public Guid OrgnizationId { get; set; }

    public List<BookV2> Books { get; set; }
}