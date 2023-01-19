using VKBot.Model;

namespace VKBot.Command.Factory;

public interface ICommandFactory
{ 
    ICommand Create(Update update,CancellationToken cancellationToken);
}