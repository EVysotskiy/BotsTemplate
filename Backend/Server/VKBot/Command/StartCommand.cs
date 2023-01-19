namespace VKBot.Command;

public class StartCommand : ICommand
{
    private readonly CommandDecorator _commandDecorator;

    private const string ANSWER_TEXT =
        "Hello!\nI'm your project assistant.\nHere you can:\n- calculate the cost of the project\n- ask questions";

    public StartCommand(CommandDecorator commandDecorator)
    {
        _commandDecorator = commandDecorator;
    }

    public async Task Execute()
    {
       await _commandDecorator.SendMessageByText(ANSWER_TEXT);
    }
}