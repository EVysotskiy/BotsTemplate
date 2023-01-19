namespace TelegramBot.Command;

public interface ICommand
{ 
    Task Execute();
}