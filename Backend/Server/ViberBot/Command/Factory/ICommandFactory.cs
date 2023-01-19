using Viber.Bot;
using Viber.Bot.NetCore.Models;

namespace ViberBot.Command.Factory;

public interface ICommandFactory
{ 
    ICommand Create(ViberCallbackData callbackData,CancellationToken cancellationToken);
}