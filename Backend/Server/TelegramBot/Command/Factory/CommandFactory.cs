using Domain.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command;

public class CommandFactory : ICommandFactory
{
    private readonly ITelegramUserServices _telegramUserServices;
    private readonly ITelegramBotClient _telegramBotClient;

    public CommandFactory(ITelegramUserServices telegramUserServices, ITelegramBotClient telegramBotClient)
    {
        _telegramUserServices = telegramUserServices;
        _telegramBotClient = telegramBotClient;
    }
    public ICommand Create(Message message,CancellationToken cancellationToken)
    {
        var commandType = GetTypeByMessage(message);

        return commandType switch
        {
            CommandType.None => new UnregisteredCommand(message,_telegramBotClient,cancellationToken),
            CommandType.StartBot => new StartCommand(message,_telegramBotClient,_telegramUserServices,cancellationToken),
            _ => throw new ArgumentException("This command type has no handler")
        };
    }

    private CommandType GetTypeByMessage(Message message)
    {
        var isText = message.Text != null;
        
        if (isText)
        {
            return message.Text switch
            {
                "/start" => CommandType.StartBot,
                "/addProject" => CommandType.AddProject,
                _ => CommandType.None
            };
        }

        return CommandType.None;
    }
}