namespace Common.Application.Authentication;

public interface IUserService
{
    Task<User> GetUser();
}