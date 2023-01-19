using Domain.Services;
using Microsoft.Extensions.Options;
using TelegramBot.Command;
using Viber.Bot;
using Viber.Bot.NetCore.Models;
using Viber.Bot.NetCore.RestApi;
using ViberBot.Configuration;

namespace ViberBot.Command.Factory;

public class CommandFactory : ICommandFactory
{
    private readonly IViberUserServices _viberUserServices;
    private readonly IViberBotApi _viberBotClient;
    private readonly ViberOptions _viberOptions;

    public CommandFactory(IViberUserServices viberUserServices, IViberBotApi viberBotClient,IOptions<ViberOptions> options)
    {
        _viberUserServices = viberUserServices;
        _viberBotClient = viberBotClient;
        _viberOptions = options.Value;
    }
    
    public ICommand Create(ViberCallbackData callbackData,CancellationToken cancellationToken)
    {
        var commandType = GetTypeByMessage(callbackData);

        return commandType switch
        {
            CommandType.None => new UnregisteredCommand(callbackData,_viberBotClient,_viberUserServices,_viberOptions),
            CommandType.StartBot => new StartCommand(callbackData,_viberBotClient,_viberUserServices,_viberOptions),
            _ => throw new ArgumentException("This command type has no handler")
        };
    }

    private CommandType GetTypeByMessage(ViberCallbackData callbackData)
    {
        if (callbackData.Message == null)
        {
            return CommandType.None;
        }
        
        //TODO: Нужно заменить
        var isText = callbackData.Message.Type is "text";

        if (isText)
        {
            var message = (ViberMessage.TextMessage)callbackData.Message;
            return message.Text switch
            {
                "/start" => CommandType.StartBot,
                "/addProject" => CommandType.AddProject,
                _ => CommandType.None
            };
        }

        return CommandType.None;
    }
}