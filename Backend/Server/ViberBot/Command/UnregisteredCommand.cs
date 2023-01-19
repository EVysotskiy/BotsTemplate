using Domain.Services;
using Viber.Bot;
using Viber.Bot.NetCore.Models;
using Viber.Bot.NetCore.RestApi;
using ViberBot.Configuration;
using ViberUser = Domain.Model.ViberUser;

namespace ViberBot.Command;

public class UnregisteredCommand : ICommand
{
    private readonly ViberCallbackData _callbackData;
    private readonly IViberBotApi _viberBotClient;
    private readonly IViberUserServices _viberUserServices;
    private readonly ViberOptions _viberOptions;

    private const string ANSWER_TEXT = "Sorry!";

    public UnregisteredCommand(ViberCallbackData callbackData, IViberBotApi viberBotClient, IViberUserServices viberUserServices, ViberOptions viberOptions)
    {
        _callbackData = callbackData;
        _viberBotClient = viberBotClient;
        _viberUserServices = viberUserServices;
        _viberOptions = viberOptions;
    }

    public async Task Execute()
    {
        var messageText = ANSWER_TEXT;

        if (_callbackData.Sender == null)
        {
            return;
        }

        var userId = _callbackData.Sender.Id;

        var textMessage = new ViberMessage.TextMessage()
        {
            Receiver = userId,
            Sender = new Viber.Bot.NetCore.Models.ViberUser.User()
            {
                Name = "Dadve",
                Avatar =
                    "https://media-direct.cdn.viber.com/download_photo?dlid=q7yAZp3m55knZjpQlQnWof-krMD1iDdTZfGvDyjoW7NYPnBMzo84OVXkT9bV6JyxUL-KB6mWJKQT9criEtjZpz07RF_rO5SaAYoSgb3gN1ktmeX8mX6IPu7c-sCtbxODjZfqQQ&fltp=jpg&imsz=0000"
            },
            Text = messageText
        };

        var response = await _viberBotClient.SendMessageAsync<ViberResponse.SendMessageResponse>(textMessage);

        var user = await _viberUserServices.Get(userId);

        if (ReferenceEquals(user, null))
        {
            var newUser = new ViberUser(userId);
            await _viberUserServices.Add(newUser);
        }
    }
}