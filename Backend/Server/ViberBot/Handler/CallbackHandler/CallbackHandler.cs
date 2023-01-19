using Viber.Bot;
using Viber.Bot.NetCore.Models;
using ViberBot.Command.Factory;

namespace ViberBot.Handler.CallbackHandler;

public class CallbackHandler : ICallbackHandler
{
    private readonly ICommandFactory _commandFactory;

    public CallbackHandler(ICommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public async Task Handle(ViberCallbackData callbackData, CancellationToken cancellationToken)
    {
        await HandleUpdate(callbackData, cancellationToken);
    }
    
    private async Task HandleUpdate(ViberCallbackData callbackData,CancellationToken cancellationToken)
    {
        var command = _commandFactory.Create(callbackData,cancellationToken);
        await command.Execute();
    }
}