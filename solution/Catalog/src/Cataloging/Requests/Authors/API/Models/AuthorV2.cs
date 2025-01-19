using Cataloging.Requests.Books.API;
using System.ComponentModel.DataAnnotations;

namespace Cataloging.Requests.Authors.API.Models;

public class AuthorV2
{
    public Guid Id { get; set; }

    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public Guid OrgnizationId { get; set; }

    public List<BookV2> Books { get; set; }
}