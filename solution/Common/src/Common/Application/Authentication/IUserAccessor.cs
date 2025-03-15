namespace Common.Application.Authentication;

public interface IUserAccessor
{
    Task<User> GetUser();
}