using Domain.Model;

namespace Domain.Services;

public interface IViberUserServices
{
    Task<ViberUser> Get(string userId);
    Task<ViberUser> Add(ViberUser newTelegramUser);
}