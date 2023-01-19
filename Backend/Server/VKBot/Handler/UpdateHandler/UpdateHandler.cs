using VKBot.Command.Factory;
using VKBot.Model;

namespace VKBot.Handler.UpdateHandler;

public class UpdateHandler : IUpdateHandler
{
    private readonly ICommandFactory _commandFactory;

    public UpdateHandler(ICommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public async Task Handle(Update callbackData, CancellationToken cancellationToken)
    {
        await HandleUpdate(callbackData, cancellationToken);
    }
    
    private async Task HandleUpdate(Update callbackData,CancellationToken cancellationToken)
    {
        var command = _commandFactory.Create(callbackData,cancellationToken);
        await command.Execute();
    }
}