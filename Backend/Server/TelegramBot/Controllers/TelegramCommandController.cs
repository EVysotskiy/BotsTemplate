using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TelegramBot.Filters;
using TelegramBot.Handler;

namespace TelegramBot.Controllers;

[ApiController]
[Route("telegram")]
[Produces("application/json")]
public class TelegramCommandController : ApiController
{
    private readonly IUpdatesHandler _updatesHandler;
    private readonly ILogger<TelegramCommandController> _logger;

    public TelegramCommandController(IUpdatesHandler updatesHandler, ILogger<TelegramCommandController> logger)
    {
        _updatesHandler = updatesHandler;
        _logger = logger;
    }

    [HttpPost]
    [ValidateTelegramBot]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Debug, $"Telegram CommandController Post update type: {update.Type}");
        await _updatesHandler.Handle(update,cancellationToken);
        return Ok();
    }
}