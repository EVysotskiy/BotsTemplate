using Domain.Model;

namespace Domain.Services;

public interface IUserServices
{
    Task<User> Get(long userId);
    Task<User> Get(string login, string password);
    Task<User> Add(User user);
    Task<User> Update(User user);
    Task<User> Get(string login);
}