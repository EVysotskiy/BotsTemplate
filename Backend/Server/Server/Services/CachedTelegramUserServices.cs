using Domain.Model;
using Domain.Services;
using Microsoft.Extensions.Caching.Memory;
using Server.Extensions;

namespace Server.Services
{
    public class CachedTelegramUserServices : ITelegramUserServices
    {
        private readonly ITelegramUserServices _telegramUserServices;
        private const string KeyPrefix = "users";
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedTelegramUserServices> _logger;

        public CachedTelegramUserServices(ITelegramUserServices telegramUserServices, IMemoryCache cache, ILogger<CachedTelegramUserServices> logger)
        {
            _telegramUserServices = telegramUserServices;
            _cache = cache;
            _logger = logger;
        }
        
        public async Task<TelegramUser> Get(long chatId)
        {
            _logger.Log(LogLevel.Information,$"Get User {KeyPrefix}:{chatId}");
            return await _cache.Remember($"{KeyPrefix}:{chatId}", async () => await _telegramUserServices.Get(chatId));
        }
        
        public async Task<TelegramUser> Add(TelegramUser newTelegramUser)
        {
            await _telegramUserServices.Add(newTelegramUser);
            _logger.Log(LogLevel.Information,$"Add User {KeyPrefix}:{newTelegramUser.IdChat}");
            _cache.Set($"{KeyPrefix}:{newTelegramUser.IdChat}", newTelegramUser);
            return newTelegramUser;
        }
    }
}