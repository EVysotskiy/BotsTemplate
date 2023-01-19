using Domain.Model;
using Domain.Services;
using Server.Repositories;

namespace Server.Services;

public class UserService : IUserServices
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(UserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<User> Get(long userId)
    {
         return await _userRepository.First(x => x.Id == userId);
    }

    public async Task<User> Get(string login, string password)
    {
        return await _userRepository.First(x => x.Email == login && x.Password == password);
    }
    
    public async Task<User> Get(string login)
    {
        return await _userRepository.First(x => x.Email == login);
    }
    
    public async Task<User> Add(User user)
    {
        return await _userRepository.Add(user);
    }

    public async Task<User> Update(User user)
    {
        return await _userRepository.Update(user);
    }
}