using System.ComponentModel.DataAnnotations;

namespace Cataloging.API.Models;

public record PostAuthorDtoV1
{
    [Required]
    public DateTime Birthdate { get; set; }

    [Required]
    [StringLength(32)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(32)]
    public string LastName { get; set; }

    [Required]
    public Guid OrganizationId { get; set; }
}