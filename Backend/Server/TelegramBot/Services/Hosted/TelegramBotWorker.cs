using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Configuration;

namespace TelegramBot.Services.Hosted;

public class TelegramBotWorker: IHostedService
{
    private readonly ILogger<TelegramBotWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TelegramOptions _botConfig;

    public TelegramBotWorker(
        ILogger<TelegramBotWorker> logger,
        IServiceProvider serviceProvider,
        IOptions<TelegramOptions> botOptions)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _botConfig = botOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        
        var webhookAddress = $"{_botConfig.HostAddress}{_botConfig.Route}";
        _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);
        await botClient.SetWebhookAsync(
            url: webhookAddress,
            allowedUpdates: Array.Empty<UpdateType>(),
            cancellationToken: cancellationToken,
            secretToken:_botConfig.SecretToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        
        _logger.LogInformation("Removing webhook");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}