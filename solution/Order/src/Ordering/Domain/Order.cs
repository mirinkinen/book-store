using System.Runtime.InteropServices;

namespace Ordering.Domain;

public class Order : Entity
{
    public Guid OrganizationId { get; set; }
}