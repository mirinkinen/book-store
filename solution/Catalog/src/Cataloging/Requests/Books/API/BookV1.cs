using Cataloging.Requests.Authors.API;
using Cataloging.Requests.Authors.Domain;

namespace Cataloging.Requests.Books.API;

public class BookV1
{
    public Guid Id { get; protected set; }
    public Guid AuthorId { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime DatePublished { get; protected set; }
    public DateTime ModifiedAt { get; protected set; }
    public Guid ModifiedBy { get; set; }
    public string Title { get; protected set; }
    public AuthorV1 Author { get; protected set; }
}