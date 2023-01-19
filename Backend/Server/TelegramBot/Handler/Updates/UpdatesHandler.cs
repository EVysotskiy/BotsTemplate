using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Command;

namespace TelegramBot.Handler;

public class UpdatesHandler : IUpdatesHandler
{
    private readonly ICommandFactory _commandFactory;

    public UpdatesHandler(ICommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public async Task Handle(IReadOnlyList<Update> updates,CancellationToken cancellationToken)
    {
        foreach (var update in updates)
        {
            await HandleUpdate(update,cancellationToken);
        }
    }
    
    public async Task Handle(Update update,CancellationToken cancellationToken)
    {
        await HandleUpdate(update,cancellationToken);
    }
    
    private async Task HandleUpdate(Update update,CancellationToken cancellationToken)
    {
        var isMessage = update.Type == UpdateType.Message;

        if (isMessage is false || ReferenceEquals(update.Message,null))
        {
            return;
        }

        var message = update.Message;
        var command = _commandFactory.Create(message,cancellationToken);
        await command.Execute();
    }
}