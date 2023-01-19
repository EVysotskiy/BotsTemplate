using Domain.Model;
using Domain.Services;
using Server.Repositories;

namespace Server.Services
{
    public class TelegramUserServices : ITelegramUserServices
    {
        
        private readonly TelegramUserRepository _telegramUserRepository;
        private readonly ILogger _logger;

        public TelegramUserServices
        (
            TelegramUserRepository telegramUserRepository, 
            ILogger<TelegramUserServices> logger)
        {
            _telegramUserRepository = telegramUserRepository;
            _logger = logger;
        }
        
        public async Task<TelegramUser> Get(long chatId)
        {
            return await _telegramUserRepository.First(user => user.IdChat == chatId);
        }
        
        public async Task<TelegramUser> Add(TelegramUser newTelegramUser)
        {
            return await _telegramUserRepository.Add(newTelegramUser);
        }
    }
}