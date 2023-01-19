using Domain.Model;
using Domain.Services;
using Server.Repositories;

namespace Server.Services;

public class VkUserService : IVkUserService
{
    private readonly VkUserRepository _vkUserRepository;
    private readonly ILogger<VkUserService> _logger;

    public VkUserService(VkUserRepository vkUserRepository, ILogger<VkUserService> logger)
    {
        _vkUserRepository = vkUserRepository;
        _logger = logger;
    }

    public async Task<VkUser> Get(long userId)
    {
        return await _vkUserRepository.First(user => user.UserId == userId);

    }

    public async Task<VkUser> Add(VkUser newUser)
    {
        return await _vkUserRepository.Add(newUser);
    }
}