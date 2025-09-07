using Microsoft.AspNetCore.Http.Features;
using TestTaskApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAdPlatformsService, AdPlatformService>();

builder.Services.AddControllers();

builder.Services.AddCors(builder =>
{
    builder.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }