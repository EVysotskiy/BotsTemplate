using Domain.Model;

namespace Domain.Services;

public interface ITelegramUserServices
{
    Task<TelegramUser> Get(long chatId);
    Task<TelegramUser> Add(TelegramUser newTelegramUser);
}