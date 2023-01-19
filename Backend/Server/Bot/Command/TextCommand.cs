namespace Bot.Command;

public class TextCommand : ICommand
{
    private readonly IBotClient _botClient;
    
    
    public TextCommand(IBotClient botClient)
    {
        _botClient = botClient;
    }

    public Task Execute()
    {

    }
}