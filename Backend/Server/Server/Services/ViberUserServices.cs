using Domain.Model;
using Domain.Services;
using Server.Repositories;

namespace Server.Services
{
    public class ViberUserServices : IViberUserServices
    {
        private readonly ViberUserRepository _viberUserRepository;
        private readonly ILogger<ViberUserServices> _logger;

        public ViberUserServices
        (
            ViberUserRepository viberUserRepository, 
            ILogger<ViberUserServices> logger)
        {
            _viberUserRepository = viberUserRepository;
            _logger = logger;
        }
        
        public async Task<ViberUser> Get(string chatId)
        {
            return await _viberUserRepository.First(user => user.UserId == chatId);
        }
        
        public async Task<ViberUser> Add(ViberUser newTelegramUser)
        {
            return await _viberUserRepository.Add(newTelegramUser);
        }
    }
}