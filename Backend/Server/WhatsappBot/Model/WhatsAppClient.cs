using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WhatsappBot.Configuration;
using WhatsappBot.Network;

namespace WhatsappBot.Model;

public interface IWhatsAppClient
{
    Task<string> SendMessage(string chatId, string text);
}

public class WhatsAppClient : IWhatsAppClient
{
    private readonly WAOptions _waOptions;
    private readonly Request _request;

    public WhatsAppClient(IOptions<WAOptions> options)
    {
        _waOptions = options.Value;
        _request = new Request(_waOptions);
    }

    public async Task<string> SendMessage(string chatId, string text)
    {
        const string nameMethod = "sendMessage";

        var data = new Dictionary<string, string>()
        {
            { "chatId", chatId },
            { "body", text }
        };
        var jsonData = JsonConvert.SerializeObject(data);
        return await _request.SendRequest(nameMethod, jsonData);
    }
}