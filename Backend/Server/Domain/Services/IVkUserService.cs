using Domain.Model;

namespace Domain.Services;

public interface IVkUserService
{
    Task<VkUser> Get(long userId);
    Task<VkUser> Add(VkUser newUser);
}