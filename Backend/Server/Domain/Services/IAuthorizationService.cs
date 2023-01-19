using Domain.Model;

namespace Domain.Services;

public interface IAuthorizationService
{
    Task<User> Registration(string login, string password);
    Task<User> Login(string login, string password);
}