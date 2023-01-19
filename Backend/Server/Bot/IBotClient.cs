public interface IBotClient
{
    string Name { get; }
    Task SendMessage(string message);
}