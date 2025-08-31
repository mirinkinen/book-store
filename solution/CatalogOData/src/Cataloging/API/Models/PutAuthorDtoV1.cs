using System.ComponentModel.DataAnnotations;

namespace Cataloging.API.Models;

public class PutAuthorDtoV1
{
    [Required]
    public DateTime Birthdate { get; set; }

    [Required]
    [StringLength(32)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(32)]
    public string LastName { get; set; }
}