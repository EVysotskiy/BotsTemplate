using WhatsappBot.Model;

namespace WhatsappBot.Command.Factory;

public interface ICommandFactory
{ 
    ICommand Create(Message message,CancellationToken cancellationToken);
}