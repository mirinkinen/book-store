namespace Shared.Application.Authentication;

public class User
{
    public Guid Id { get; }

    public IEnumerable<Guid> Organizations { get; }

    public User(Guid id, IEnumerable<Guid> organizations)
    {
        Id = id;
        Organizations = organizations ?? throw new ArgumentNullException(nameof(organizations));
    }
}