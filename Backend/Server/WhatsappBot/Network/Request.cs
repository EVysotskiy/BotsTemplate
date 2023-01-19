using System.Text;
using Microsoft.Extensions.Options;
using WhatsappBot.Configuration;

namespace WhatsappBot.Network;

public class Request
{
    private readonly WAOptions _waOptions;

    public Request(WAOptions options)
    {
        _waOptions = options;
    }

    public async Task<string> SendRequest(string method, string data)
    {
        string url = $"{_waOptions.UrlApi}{method}?token={_waOptions.TokenBot}";

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(url);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("", content);
            return await result.Content.ReadAsStringAsync();
        }
    }
}