using System.ComponentModel.DataAnnotations;

namespace Cataloging.API.Models;

public class AuthorV1
{
    public Guid Id { get; set; }

    [DataType(DataType.Date)]
    public DateTime Birthdate { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    [MaxLength(32)]
    public string FirstName { get; set; }

    [MaxLength(32)]
    public string LastName { get; set; }

    public Guid OrganizationId { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }

    public Guid ModifiedBy { get; set; }

    public IReadOnlyList<BookV1> Books { get; } = new List<BookV1>();
}