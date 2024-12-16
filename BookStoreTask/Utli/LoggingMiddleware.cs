using System.Text;

namespace BookStoreTask.Utli;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the incoming request
        await LogRequest(context);

        // Capture the response
        var originalResponseBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            // Call the next middleware in the pipeline
            await _next(context);

            // Log the response
            await LogResponse(context);

            // Copy the response back to the original stream
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
        finally
        {
            context.Response.Body = originalResponseBodyStream;
        }
    }

    private async Task LogRequest(HttpContext context)
    {
        context.Request.EnableBuffering();
        var bodyAsText = await ReadStreamAsync(context.Request.Body);
        context.Request.Body.Position = 0;

        _logger.LogInformation("Incoming Request");
        _logger.LogInformation("Path: {Path}", context.Request.Path);
        _logger.LogInformation("Method: {Method}", context.Request.Method);
        _logger.LogInformation("Headers: {Headers}", context.Request.Headers);
        if (!string.IsNullOrEmpty(bodyAsText))
        {
            _logger.LogInformation("Body: {Body}", bodyAsText);
        }
    }

    private async Task LogResponse(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var bodyAsText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation("Response");
        _logger.LogInformation("Status Code: {StatusCode}", context.Response.StatusCode);
        if (!string.IsNullOrEmpty(bodyAsText))
        {
            _logger.LogInformation("Body: {Body}", bodyAsText);
        }
    }

    private async Task<string> ReadStreamAsync(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(stream, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
        stream.Seek(0, SeekOrigin.Begin);
        return text;
    }
}