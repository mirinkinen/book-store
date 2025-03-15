namespace Common.Application.Authentication;

public class UserAccessor : IUserAccessor
{
    public Task<User> GetUser()
    {
        var organizations = new[] {
            Guid.Parse("5D8E6753-1479-408E-BB3D-CB3A02BE486C"),
            Guid.Parse("284F633F-2D13-4F4D-8E37-1EE5C9F6B140")
        };

        return Task.FromResult(new User(Guid.Parse("893A5338-6BE9-4C95-831C-7F4A1816EA2B"), organizations));
    }
}