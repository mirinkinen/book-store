using System.ComponentModel.DataAnnotations;

namespace Cataloging.Requests.Authors.API.Models;

public record PostAuthorDtoV1
{
    [Required]
    public DateTime? Birthday { get; set; }

    [Required]
    [StringLength(32)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(32)]
    public string? LastName { get; set; }

    [Required]
    public Guid? OrganizationId { get; set; }
}