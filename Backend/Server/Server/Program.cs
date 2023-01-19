using Domain.Services;
using MailDelivery.Model;
using MailDelivery.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Database;
using Server.Options;
using Server.Repositories;
using Server.Services;
using Telegram.Bot;
using TelegramBot.Command;
using TelegramBot.Configuration;
using TelegramBot.Handler;
using TelegramBot.Services.Hosted;
using Viber.Bot.NetCore.Middleware;
using ViberBot.Configuration;
using ViberBot.Handler.CallbackHandler;
using ViberBot.Services.Hosted;
using VKBot.Configuration;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;
using WhatsappBot.Configuration;
using WhatsappBot.Model;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


//Registration bots
{
    builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x =>
        new TelegramBotClient(x.GetService<IOptions<TelegramOptions>>()?.Value.TokenBot ?? string.Empty));

    builder.Services.AddViberBotApi(x =>
    {
        var viberOptions = configuration.GetSection(ViberOptions.Position).Get<ViberOptions>();
        x.Token = viberOptions.TokenBot;
        x.Webhook = $"{viberOptions.HostAddress}{viberOptions.Route}";
    });

    builder.Services.AddSingleton<IVkApi, VkApi>(x =>
    {
        var api = new VkApi();
        api.Authorize(new ApiAuthParams() { AccessToken = x.GetService<IOptions<VkOptions>>()?.Value.TokenBot ?? string.Empty });
        return api;
    });

    builder.Services.AddSingleton<IWhatsAppClient, WhatsAppClient>();
}

//Options
{
    builder.Services.Configure<TelegramOptions>(configuration.GetSection(TelegramOptions.Position));
    builder.Services.Configure<ViberOptions>(configuration.GetSection(ViberOptions.Position));
    builder.Services.Configure<VkOptions>(configuration.GetSection(VkOptions.Position));
    builder.Services.Configure<WAOptions>(configuration.GetSection(WAOptions.Position));
    builder.Services.Configure<JWT>(configuration.GetSection(JWT.Position));
    builder.Services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.Position));
}
var jwtConfig = builder.Configuration.GetSection(JWT.Position).Get<JWT>();

// Services
{
    //MailDelivery
    {
        builder.Services.AddScoped<IEmailService, EmailService>();
    }
    
    builder.Services.AddScoped<IUserServices, UserService>();
    builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
    builder.Services.AddScoped<ITelegramUserServices, TelegramUserServices>();
    builder.Services.AddScoped<IViberUserServices, ViberUserServices>();
    builder.Services.AddScoped<IVkUserService, VkUserService>();
    builder.Services.Decorate<ITelegramUserServices, CachedTelegramUserServices>();
    builder.Services.AddHostedService<TelegramBotWorker>();
    builder.Services.AddHostedService<ViberBotWorker>();
}

//TelegramCommand
{
    builder.Services.AddTransient<ICommandFactory, CommandFactory>();
    builder.Services.AddScoped<IUpdatesHandler, UpdatesHandler>();
}

//ViberCommand
{
    builder.Services.AddTransient<ViberBot.Command.Factory.ICommandFactory, ViberBot.Command.Factory.CommandFactory>();
    builder.Services.AddScoped<ICallbackHandler, CallbackHandler>();
}

//VkCommand
{
    builder.Services.AddTransient<VKBot.Command.Factory.ICommandFactory, VKBot.Command.Factory.CommandFactory>();
    builder.Services.AddScoped<VKBot.Handler.UpdateHandler.IUpdateHandler, VKBot.Handler.UpdateHandler.UpdateHandler>();
}

//WACommand
{
    builder.Services.AddTransient<WhatsappBot.Command.Factory.ICommandFactory, WhatsappBot.Command.Factory.CommandFactory>();
    builder.Services.AddScoped<WhatsappBot.Handler.UpdateHandler.IUpdateHandler, WhatsappBot.Handler.UpdateHandler.UpdateHandler>();
}

//Repository
{
    builder.Services.AddScoped<UserRepository>();
    builder.Services.AddScoped<TelegramUserRepository>();
    builder.Services.AddScoped<ViberUserRepository>();
    builder.Services.AddScoped<VkUserRepository>();
    builder.Services.AddScoped<WAUserRepository>();
}

builder.Services.AddDbContextFactory<AppDbContext>(ConfigurePostgresConnection);
builder.Services.AddDbContext<AppDbContext>(ConfigurePostgresConnection);
builder.Services.AddMemoryCache();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.ToString());
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = jwtConfig.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

void ConfigurePostgresConnection(DbContextOptionsBuilder options)
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlContext"));
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials
app.Run();