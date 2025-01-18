using Cataloging.Requests.Authors.API;
using Cataloging.Requests.Authors.Domain;

namespace Cataloging.Requests.Books.API;

public class BookV2
{
    public Guid Id { get; protected set; }
    public Guid AuthorId { get; protected set; }
    public AuthorV2 Author { get; protected set; }
    public DateTime DatePublished { get; protected set; }
    public string Title { get; protected set; }
    public decimal Price { get; protected set; }
}