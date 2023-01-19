using WhatsappBot.Command.Factory;
using WhatsappBot.Model;

namespace WhatsappBot.Handler.UpdateHandler;

public class UpdateHandler : IUpdateHandler
{
    private readonly ICommandFactory _commandFactory;

    public UpdateHandler(ICommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        foreach (var updateMessage in update.entry)
        {
            foreach (var updateMessageChange in updateMessage.changes)
            {
                foreach (var valueMessage in updateMessageChange.value.Messages)
                {
                    await HandleUpdate(valueMessage, cancellationToken);
                }


            }
        }
    }
    
    private async Task HandleUpdate(Message callbackData,CancellationToken cancellationToken)
    {
        var command = _commandFactory.Create(callbackData,cancellationToken);
        await command.Execute();
    }
}