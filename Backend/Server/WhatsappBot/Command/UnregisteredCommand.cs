namespace WhatsappBot.Command;

public class UnregisteredCommand : ICommand
{
    private readonly CommandDecorator _commandDecorator;
    private const string ANSWER_TEXT = "Sorry!";

    public UnregisteredCommand(CommandDecorator commandDecorator)
    {
        _commandDecorator = commandDecorator;
    }
    
    public async Task Execute()
    {
        await _commandDecorator.SendMessageByText(ANSWER_TEXT);
    }
}