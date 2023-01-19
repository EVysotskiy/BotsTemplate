using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command;

public class UnregisteredCommand : ICommand
{
    private readonly Message _message;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly CancellationToken _cancellationToken;

    private const string ANSWER_TEXT = "Sorry! I don't know this command.";
    
    public UnregisteredCommand(Message message, ITelegramBotClient telegramBotClient,CancellationToken cancellationToken)
    {
        _message = message;
        _telegramBotClient = telegramBotClient;
        _cancellationToken = cancellationToken;
    }

    public async Task Execute()
    {
        var messageText = ANSWER_TEXT;
        var chatId = _message.Chat.Id;
        await _telegramBotClient.SendTextMessageAsync(chatId, messageText, cancellationToken: _cancellationToken);
    }
}