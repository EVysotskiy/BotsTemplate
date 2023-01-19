using Telegram.Bot.Types;

namespace TelegramBot.Handler;

public interface IUpdatesHandler
{
    Task Handle(IReadOnlyList<Update> updates,CancellationToken cancellationToken);
    Task Handle(Update updates, CancellationToken cancellationToken);
}