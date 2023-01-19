using Domain.Model;

namespace Domain.Services;

public interface IWAUserService
{
    Task<WAUser> Get(string phoneNumber);
    Task<WAUser> Add(WAUser newTelegramUser);
}