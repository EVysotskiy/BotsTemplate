using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Model;
using Domain.Services;
using MailDelivery.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Options;

namespace Server.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserServices _userServices;
    private readonly ILogger<AuthorizationService> _logger;
    private readonly IEmailService _emailService;
    private readonly JWT _optionsJwt;

    public AuthorizationService(IUserServices userServices, ILogger<AuthorizationService> logger, IOptions<JWT> optionsJwt, IEmailService emailService)
    {
        _userServices = userServices;
        _logger = logger;
        _emailService = emailService;
        _optionsJwt = optionsJwt.Value;
    }

    public async Task<User> Registration(string login, string password)
    {
        var hashPassword = GetHash(password);
        var user = await _userServices.Get(login);
        
        if (ReferenceEquals(user,null) is false)
        {
            return null;
        }

        user = new User(login, hashPassword);
        user = await _userServices.Add(user);
        var jwt = GenerateJwt(user);
        user.SetSession(jwt);
        await _userServices.Update(user);
        await _emailService.SendEmail(login, "Регистрация", "Чувак, ты зарегался");
        return user;
    }

    public async Task<User> Login(string login, string password)
    {
        var user = await _userServices.Get(login);
        var isPasswordVerify = IsVerify(password, user.Password);
        if (isPasswordVerify is false)
        {
            _logger.Log(LogLevel.Debug, $"Invalid password:\nlogin: {login}\npassword: {password}");
            return null;
        }
        
        if (ReferenceEquals(user,null))
        {
            _logger.Log(LogLevel.Debug, $"Login attempt:\nlogin: {login}\npassword: {password}");
            return null;
        }

        return user;
    }
    
    
    private string GenerateJwt([NotNull] User user)
    {
        var now = DateTime.UtcNow;
        var identity = GetIdentity(user);
        var jwt = new JwtSecurityToken(
            issuer: _optionsJwt.Issuer,
            audience: _optionsJwt.Audience,
            notBefore: now,
            claims: identity.Claims,
            expires: now.AddHours(_optionsJwt.LifetimeHour),
            signingCredentials: new SigningCredentials(_optionsJwt.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        _logger.Log(LogLevel.Information, $"Auth user {user.Id}");
        return encodedJwt;
    }


    private ClaimsIdentity GetIdentity(User user)
    {
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        // если пользователя не найдено
        return null;
    }

    private string GetHash(string value)
    {
        return BCrypt.Net.BCrypt.HashPassword(value);
    }

    private bool IsVerify(string value, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(value, hash);
    }
}