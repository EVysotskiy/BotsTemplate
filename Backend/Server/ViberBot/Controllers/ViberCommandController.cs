using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Viber.Bot;
using Viber.Bot.NetCore.Models;
using ViberBot.Handler.CallbackHandler;

namespace ViberBot.Controllers;


[ApiController]
[Route("viber")]
[Produces("application/json")]
public class ViberCommandController : ApiController
{
    private readonly ILogger<ViberCommandController> _logger;
    private readonly ICallbackHandler _callbackHandler;

    public ViberCommandController(ILogger<ViberCommandController> logger, ICallbackHandler callbackHandler)
    {
        _logger = logger;
        _callbackHandler = callbackHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ViberCallbackData data, CancellationToken cancellationToken)
    {
        //TODO:Так делать нельзя, нужно что - то придумать
        if (data.Event == "webhook")
        {
            return Ok();
        }
        _logger.Log(LogLevel.Debug, $"Viber CommandController Post Context: {data.Context}");
        await _callbackHandler.Handle(data,cancellationToken);
        return Ok();
    }
}