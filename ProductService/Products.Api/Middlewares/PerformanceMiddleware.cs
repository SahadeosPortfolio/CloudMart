using System.Diagnostics;

namespace Products.Api.Middlewares;

public class PerformanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceMiddleware> _logger;
    private readonly Stopwatch _stopwatch;
    public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _stopwatch = new Stopwatch();
    }
    public async Task InvokeAsync(HttpContext context)
    {
        _stopwatch.Start();
        await _next(context);
        _stopwatch.Stop();

        var elapsedMs = _stopwatch.ElapsedMilliseconds;
        var statusCode = context.Response.StatusCode;
        var path = context.Request.Path;

        if (elapsedMs > 500) // Log if request takes longer than 500ms
        {
            _logger.LogInformation("Request {Method} {Path} responded {StatusCode} in {Elapsed}ms",
            context.Request.Method, path, statusCode, elapsedMs);
        }
    }
}
