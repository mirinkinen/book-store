using Books.Api.Domain.SeedWork;

namespace Books.Api.Domain.Authors;

public class Author : Entity
{
    public string Firstname { get; internal set; }

    public string Lastname { get; internal set; }

    public DateTime Birthday { get; internal set; }

    public Author(string firstname, string lastname, DateTime birthday)
    {
        Firstname = firstname;
        Lastname = lastname;
        Birthday = birthday;
    }
}