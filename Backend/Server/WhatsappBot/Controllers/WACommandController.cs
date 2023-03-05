

using System.Web;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WhatsappBot.Configuration;
using WhatsappBot.Handler.UpdateHandler;
using WhatsappBot.Model;

namespace WhatsappBot.Controllers;

[ApiController]
[Route("wa")]
[Produces("application/json")]
public class WACommandController : ApiController
{
    private readonly WAOptions _waOptions;
    private readonly ILogger<WACommandController> _logger;
    private readonly IUpdateHandler _updateHandler;

    public WACommandController(ILogger<WACommandController> logger, IUpdateHandler updateHandler,IOptions<WAOptions> options)
    {
        _logger = logger;
        _updateHandler = updateHandler;
        _waOptions = options.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update,CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Debug, $"WA CommandController");
        await _updateHandler.Handle(update,cancellationToken);
        return Ok("ok");
    }
    
    [HttpGet]
    public async Task<IActionResult> Webhook()
    {
        _logger.Log(LogLevel.Debug, $"WA Webhook");

        var uri = Request.RequestUri;
        var token = HttpUtility.ParseQueryString(uri.Query).Get("hub.verify_token");
        var challenge = HttpUtility.ParseQueryString(uri.Query).Get("hub.challenge");

        //"12345678qwerty" replace with the value that you specify in your account
        if (token == "12345678qwerty")
        {
            return Ok(challenge);
        }
        
        return Ok();
    }
}