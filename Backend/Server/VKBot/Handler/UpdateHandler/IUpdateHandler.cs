using VKBot.Model;

namespace VKBot.Handler.UpdateHandler;

public interface IUpdateHandler
{
    Task Handle(Update callbackData, CancellationToken cancellationToken);
}