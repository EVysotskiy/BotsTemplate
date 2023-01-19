using Domain.Services;
using Microsoft.Extensions.Options;
using Viber.Bot;
using Viber.Bot.NetCore.Models;
using Viber.Bot.NetCore.RestApi;
using ViberBot.Configuration;
using ViberUser = Domain.Model.ViberUser;

namespace ViberBot.Command;

public class StartCommand : ICommand
{
    private readonly ViberCallbackData _callbackData;
    private readonly IViberBotApi _viberBotClient;
    private readonly IViberUserServices _viberUserServices;
    private readonly ViberOptions _viberOptions;

    private const string ANSWER_TEXT =
        "Hello!\nI'm your project assistant.\nHere you can:\n- calculate the cost of the project\n- ask questions";

    public StartCommand(ViberCallbackData callbackData, IViberBotApi viberBotClient, IViberUserServices viberUserServices, ViberOptions options)
    {
        _callbackData = callbackData;
        _viberBotClient = viberBotClient;
        _viberUserServices = viberUserServices;
        _viberOptions = options;
    }

    public async Task Execute()
    {
        var messageText = ANSWER_TEXT;
        var userId = _callbackData.User.Id;
        var textMessage = new ViberMessage.TextMessage()
        {
            Receiver = _viberOptions.Id,
            Sender = _callbackData.Sender,
            Text = ANSWER_TEXT,
            TrackingData = _callbackData.Message.TrackingData,
            MinApiVersion = _callbackData.Message.MinApiVersion
        };
        
        await _viberBotClient.SendMessageAsync<ViberResponse.SendMessageResponse>(textMessage);

        var user = await _viberUserServices.Get(userId);

        if (ReferenceEquals(user,null))
        {
           var  newUser = new ViberUser(userId);
           await _viberUserServices.Add(newUser);
        }
    }
}