using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Viber.Bot;
using Viber.Bot.NetCore.Models;
using Viber.Bot.NetCore.RestApi;
using ViberBot.Configuration;

namespace ViberBot.Services.Hosted;

public class ViberBotWorker: IHostedService
{
    private readonly ILogger<ViberBotWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ViberOptions _botConfig;

    public ViberBotWorker(
        ILogger<ViberBotWorker> logger,
        IServiceProvider serviceProvider,
        IOptions<ViberOptions> botOptions)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _botConfig = botOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<IViberBotApi>();
        
        var webhookAddress = $"{_botConfig.HostAddress}{_botConfig.Route}";
        _logger.LogInformation("Viber Setting webhook: {WebhookAddress}", webhookAddress);
        await botClient.SetWebHookAsync(new ViberWebHook.WebHookRequest(webhookAddress));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<IViberBotApi>();
        
        _logger.LogInformation("Viber Removing webhook");
        await botClient.SetWebHookAsync(new ViberWebHook.WebHookRequest(String.Empty));
    }
}