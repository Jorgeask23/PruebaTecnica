using System.Security.Cryptography;
using System.Text;

namespace PruebaTecnica.Api.Middlewares;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-API-KEY", out var apiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key no proporcionada.");
            return;
        }

        var configuredApiKey = _configuration["ApiKey"];

        if (string.IsNullOrEmpty(configuredApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key no configurada.");
            return;
        }

        var providedBytes = Encoding.UTF8.GetBytes(apiKey.ToString());
        var configuredBytes = Encoding.UTF8.GetBytes(configuredApiKey);

        var isValid = providedBytes.Length == configuredBytes.Length &&
                      CryptographicOperations.FixedTimeEquals(
                          providedBytes,
                          configuredBytes);

        if (!isValid)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key inválida.");
            return;
        }

        await _next(context);
    }
}
