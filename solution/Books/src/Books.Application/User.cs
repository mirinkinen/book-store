using System.Diagnostics.CodeAnalysis;

namespace Books.Application;

public class User
{
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This is mock object")]
    public Guid Id => Guid.Parse("893A5338-6BE9-4C95-831C-7F4A1816EA2B");

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This is mock object")]
    public IEnumerable<Guid> Organizations => new[] {
        Guid.Parse("5D8E6753-1479-408E-BB3D-CB3A02BE486C"),
        Guid.Parse("284F633F-2D13-4F4D-8E37-1EE5C9F6B140")
    };
}