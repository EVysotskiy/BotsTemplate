using Domain.Model;
using Domain.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command;

public class StartCommand : ICommand
{
    private readonly Message _message;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ITelegramUserServices _telegramUserServices;
    private readonly CancellationToken _cancellationToken;

    private const string ANSWER_TEXT =
        "Hello!\nI'm your project assistant.\nHere you can:\n- calculate the cost of the project\n- ask questions";
    
    public StartCommand(Message message, ITelegramBotClient telegramBotClient, ITelegramUserServices telegramUserServices,CancellationToken cancellationToken)
    {
        _message = message;
        _telegramBotClient = telegramBotClient;
        _telegramUserServices = telegramUserServices;
        _cancellationToken = cancellationToken;
    }

    public async Task Execute()
    {
        var messageText = ANSWER_TEXT;
        var chatId = _message.Chat.Id;
        await _telegramBotClient.SendTextMessageAsync(chatId, messageText, cancellationToken: _cancellationToken);

        var user = await _telegramUserServices.Get(chatId);

        if (ReferenceEquals(user,null))
        {
           var  newUser = new TelegramUser(chatId);
           await _telegramUserServices.Add(newUser);
        }
    }
}