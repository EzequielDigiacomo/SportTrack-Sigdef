using Microsoft.AspNetCore.Http;
using SportTrack_Sigdef.Controladores.Audit;
using SportTrack_Sigdef.Controladores.Exceptions;
using System.Net;
using System.Text.Json;

namespace SportTrack_Sigdef.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, IAuditService auditService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var isExpectedBusinessError = ex is UnauthorizedException or BadRequestException or NotFoundException;

            if (isExpectedBusinessError)
            {
                _logger.LogWarning(ex, ex.Message);
            }
            else
            {
                _logger.LogError(ex, ex.Message);
                await auditService.RegistrarErrorAsync(ex, context.Request.Path);
            }

            ApplyCorsHeaders(context);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                BadRequestException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = ex.Message,
                innerMessage = _env.IsDevelopment() ? GetFullInnerException(ex) : null,
                details = _env.IsDevelopment() ? ex.StackTrace : null
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }

    private static void ApplyCorsHeaders(HttpContext context)
    {
        var origin = context.Request.Headers.Origin.ToString();
        if (string.IsNullOrEmpty(origin)) return;

        context.Response.Headers["Access-Control-Allow-Origin"] = origin;
        context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
        context.Response.Headers.Vary = "Origin";
    }

    private static string? GetFullInnerException(Exception ex)
    {
        if (ex.InnerException == null) return null;

        var inner = ex.InnerException;
        var message = inner.Message;
        while (inner.InnerException != null)
        {
            inner = inner.InnerException;
            message += " --> " + inner.Message;
        }
        return message;
    }
}
