using System.Diagnostics.CodeAnalysis;

namespace Books.Application;

public class UserService
{
    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "This is mock code")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This is mock code")]
    public User GetUser()
    {
        return new();
    }
}