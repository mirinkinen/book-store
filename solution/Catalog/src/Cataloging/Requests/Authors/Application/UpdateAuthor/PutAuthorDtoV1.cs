using System.ComponentModel.DataAnnotations;

namespace Cataloging.Requests.Authors.Application.UpdateAuthor;

public class PutAuthorDtoV1
{
    [Required]
    public DateTime? Birthday { get; set; }

    [Required]
    [StringLength(32)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(32)]
    public string LastName { get; set; }
}