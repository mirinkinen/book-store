using Books.Domain.Books;
using Books.Domain.SeedWork;
using System.Collections.ObjectModel;

namespace Books.Domain.Authors;

public class Author : Entity
{
    public string Firstname { get; private set; }

    public string Lastname { get; private set; }

    public DateTime Birthday { get; private set; }

    public ReadOnlyCollection<Book> Books { get; private set; } = new List<Book>().AsReadOnly();

    public Guid OrganizationId { get; private set; }

    public Author(string firstname, string lastname, DateTime birthday, Guid organizationId)
    {
        if(organizationId == Guid.Empty)
        {
            throw new DomainRuleException("Empty organization ID not allowed.");
        }

        Firstname = firstname;
        Lastname = lastname;
        Birthday = birthday;
        OrganizationId = organizationId;
    }
}