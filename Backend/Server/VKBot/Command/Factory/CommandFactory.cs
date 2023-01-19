using Domain.Services;
using Microsoft.Extensions.Options;
using VKBot.Configuration;
using VKBot.Model;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Utils;

namespace VKBot.Command.Factory;

public class CommandFactory : ICommandFactory
{
    private readonly IVkUserService _vkUserServices;
    private readonly IVkApi _vkBotClient;
    private readonly VkOptions _vkOptions;

    public CommandFactory(IVkUserService vkUserServices, IVkApi vkBotClient,IOptions<VkOptions> options)
    {
        _vkUserServices = vkUserServices;
        _vkBotClient = vkBotClient;
        _vkOptions = options.Value;
    }
    
    public ICommand Create(Update update,CancellationToken cancellationToken)
    {
        var commandType = GetTypeByMessage(update);


        var commandDecorator = new CommandDecorator(update, _vkBotClient, _vkUserServices, _vkOptions);
        return commandType switch
        {
            CommandType.None => new UnregisteredCommand(commandDecorator),
            CommandType.StartBot => new StartCommand(commandDecorator),
            _ => throw new ArgumentException("This command type has no handler")
        };
    }

    private CommandType GetTypeByMessage(Update update)
    {
        if (update.Type != "message_new")
        {
            return CommandType.None;
        }
        
        var message = Message.FromJson(new VkResponse(update.Object));
        if (message.Text == null)
        {
            return CommandType.None;
        }
        
        return message.Text switch
        {
            "/start" => CommandType.StartBot,
            "/addProject" => CommandType.AddProject,
            _ => CommandType.None
        };
    }
}