using WhatsappBot.Configuration;
using WhatsappBot.Model;

namespace WhatsappBot.Command;

public class CommandDecorator
{
    private readonly Message _message;
    private readonly IWhatsAppClient _whatsAppClient;
    private readonly WAOptions _waOptions;

    public CommandDecorator(Message message, IWhatsAppClient whatsAppClient, WAOptions waOptions)
    {
        _message = message;
        _whatsAppClient = whatsAppClient;
        _waOptions = waOptions;
    }

    internal async Task SendMessageByText(string messageText)
    {
        if (messageText == null)
        {
            throw new Exception("MessageText is null!");
        }

        var chatId = _message.id;
        
        await _whatsAppClient.SendMessage(chatId,messageText);
    }
}