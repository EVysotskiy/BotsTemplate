namespace TelegramBot.Configuration;

public class TelegramOptions
{
    public const string Position = "Telegram";
    public string TokenBot { get; set; }
    public string SecretToken { get; set; }
    public string HostAddress { get; set; }
    public string Route { get; set; }
}