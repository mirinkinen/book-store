using Cataloging.IntegrationTests.Books;

namespace Cataloging.IntegrationTests.Authors;

internal class AuthorViewmodel : EntityViewmodel
{
    public DateTime? Birthday { get; set; }
    public IList<BookViewmodel>? Books { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Guid? OrganizationId { get; set; }
}