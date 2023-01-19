using System.Web.Http;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = Domain.Services.IAuthorizationService;

namespace Server.Controllers;

[ApiController]
[Route("authorization")]
[Produces("application/json")]
public class AuthorizationController : ApiController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<AuthorizationController> _logger;

    public AuthorizationController(IAuthorizationService authorizationService, ILogger<AuthorizationController> logger)
    {
        _authorizationService = authorizationService;
        _logger = logger;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthorizationData authorizationData)
    {
        _logger.Log(LogLevel.Debug,$"Login user {authorizationData}");
        var user = await _authorizationService.Login(authorizationData.Login, authorizationData.Password);
        if (ReferenceEquals(user,null))
        {
            return BadRequest("User not found");
        }

        return Json(new AuthKey(user.Session));
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Registration(AuthorizationData authorizationData)
    {
        _logger.Log(LogLevel.Debug,$"Registration user {authorizationData}");
        var user = await _authorizationService.Registration(authorizationData.Login, authorizationData.Password);
        if (ReferenceEquals(user,null))
        {
            return BadRequest("Error");
        }

        return Json(new AuthKey(user.Session));
    }
    
    [Authorize]
    [HttpGet("status")]
    public async Task<IActionResult> Status(AuthorizationData authorizationData)
    {
        _logger.Log(LogLevel.Debug,$"Registration user {authorizationData}");
        var user = await _authorizationService.Registration(authorizationData.Login, authorizationData.Password);
        if (ReferenceEquals(user,null))
        {
            return BadRequest("Error");
        }

        return Json(new AuthKey(user.Session));
    }
    
}