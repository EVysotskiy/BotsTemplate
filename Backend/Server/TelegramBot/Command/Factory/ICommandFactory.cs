using Telegram.Bot.Types;

namespace TelegramBot.Command;

public interface ICommandFactory
{ 
    ICommand Create(Message message,CancellationToken cancellationToken);
}