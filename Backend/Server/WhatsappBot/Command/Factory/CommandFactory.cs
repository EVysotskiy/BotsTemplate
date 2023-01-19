using Microsoft.Extensions.Options;
using WhatsappBot.Configuration;
using WhatsappBot.Model;

namespace WhatsappBot.Command.Factory;

public class CommandFactory : ICommandFactory
{
    private readonly IWhatsAppClient _whatsAppClient;
    private readonly WAOptions _waOptions;

    public CommandFactory(IWhatsAppClient whatsAppClient,IOptions<WAOptions> options)
    {
        _whatsAppClient = whatsAppClient;
        _waOptions = options.Value;
    }
    
    public ICommand Create(Message message,CancellationToken cancellationToken)
    {
        var commandType = GetTypeByMessage(message);
        
        var commandDecorator = new CommandDecorator(message, _whatsAppClient, _waOptions);
        return commandType switch
        {
            CommandType.None => new UnregisteredCommand(commandDecorator),
            _ => throw new ArgumentException("This command type has no handler")
        };
    }

    private CommandType GetTypeByMessage(Message message)
    {
        if (message.text == null)
        {
            return CommandType.None;
        }
        
        return message.text.body switch
        {
            _ => CommandType.None
        };
    }
}