using Viber.Bot;
using Viber.Bot.NetCore.Models;

namespace ViberBot.Handler.CallbackHandler;

public interface ICallbackHandler
{
    Task Handle(ViberCallbackData callbackData, CancellationToken cancellationToken);
}