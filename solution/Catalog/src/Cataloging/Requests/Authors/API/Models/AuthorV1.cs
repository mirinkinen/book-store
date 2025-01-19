using Cataloging.Requests.Books.API;
using System.ComponentModel.DataAnnotations;

namespace Cataloging.Requests.Authors.API.Models;

public class AuthorV1
{
    public Guid Id { get; set; }

    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public Guid OrganizationId { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }

    public Guid ModifiedBy { get; set; }

    public List<BookV1> Books { get; set; }
}