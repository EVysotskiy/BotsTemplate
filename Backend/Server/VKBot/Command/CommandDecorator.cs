using Domain.Services;
using VKBot.Configuration;
using VKBot.Model;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VKBot.Command;

public class CommandDecorator
{
    private readonly Update _update;
    private readonly IVkApi _vkBotClient;
    private readonly IVkUserService _vkUserServices;
    private readonly VkOptions _vkOptions;

    public CommandDecorator(Update update, IVkApi vkBotClient, IVkUserService vkUserServices, VkOptions vkOptions)
    {
        _update = update;
        _vkBotClient = vkBotClient;
        _vkUserServices = vkUserServices;
        _vkOptions = vkOptions;
    }

    internal async Task SendMessageByText(string mesageText)
    {
        var message = Message.FromJson(new VkResponse(_update.Object));

        if (message.PeerId == null)
        {
            throw new Exception("PeerId is null!");
        }
        
        if (mesageText == null)
        {
            throw new Exception("MessageText is null!");
        }

        var messageParam = new MessagesSendParams
        {
            RandomId = message.Id.Value,
            PeerId = message.PeerId.Value,
            Message = mesageText
        };
        await _vkBotClient.Messages.SendAsync(messageParam);
    }
}