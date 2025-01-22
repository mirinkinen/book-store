using Cataloging.Requests.Books.API;
using System.ComponentModel.DataAnnotations;

namespace Cataloging.Requests.Authors.API.Models;

public class AuthorV2
{
    public Guid Id { get; set; }

    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }

    [MaxLength(32)]
    public string FirstName { get; set; }

    [MaxLength(32)]
    public string LastName { get; set; }

    public Guid OrgnizationId { get; set; }

    public IReadOnlyList<BookV2> Books { get; } = new List<BookV2>();
}