using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VKBot.Configuration;
using VKBot.Handler.UpdateHandler;
using VKBot.Model;

namespace VKBot.Controllers;

[ApiController]
[Route("vk")]
[Produces("application/json")]
public class VkCommandController : ApiController
{
    private readonly VkOptions _vkOptions;
    private readonly ILogger<VkCommandController> _logger;
    private readonly IUpdateHandler _updateHandler;

    public VkCommandController(ILogger<VkCommandController> logger, IUpdateHandler updateHandler,IOptions<VkOptions> options)
    {
        _logger = logger;
        _updateHandler = updateHandler;
        _vkOptions = options.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update data, CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Debug, $"Vk CommandController Post data.Type: {data.Type}");
        
        if (data.Type == "confirmation")
        {
            _logger.Log(LogLevel.Debug, $"Vk CommandController confirmation");
            return Ok(_vkOptions.Confirmation.Trim( '"' ));
        }
        
        await _updateHandler.Handle(data,cancellationToken);
        return Ok("ok");
    }
}