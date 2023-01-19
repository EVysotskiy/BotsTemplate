using WhatsappBot.Model;

namespace WhatsappBot.Handler.UpdateHandler;

public interface IUpdateHandler
{
    Task Handle(Update update, CancellationToken cancellationToken);
}