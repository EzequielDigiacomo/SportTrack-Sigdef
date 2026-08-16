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

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
                // Detalle técnico solo en logs / auditoría — nunca en la respuesta al cliente
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

            // Solo mensaje amigable. Sin innerException / stack hacia el front.
            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = ResolveUserMessage(ex)
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

        var allowed = context.RequestServices.GetService(typeof(CorsAllowedOrigins)) as CorsAllowedOrigins;
        if (allowed == null || !allowed.IsAllowed(origin)) return;

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

    /// <summary>
    /// Mensaje seguro para UI. Los errores de negocio (BadRequest/NotFound/Unauthorized)
    /// ya vienen redactados en español. El resto nunca expone texto técnico.
    /// </summary>
    private static string ResolveUserMessage(Exception ex)
    {
        if (ex is BadRequestException or NotFoundException or UnauthorizedException)
        {
            // Si por error alguien lanzó un mensaje técnico en negocio, sanitizar
            return IsSafeBusinessMessage(ex.Message)
                ? ex.Message
                : "No se pudo completar la operación. Revisá los datos e intentá nuevamente.";
        }

        var full = (ex.Message + " " + (GetFullInnerException(ex) ?? string.Empty)).ToLowerInvariant();

        if (full.Contains("ix_usuarios_email") || (full.Contains("email") && (full.Contains("unique") || full.Contains("duplicate"))))
            return "Ese email ya está en uso. Probá con otro.";

        if (full.Contains("ix_usuarios_username") || (full.Contains("username") && (full.Contains("unique") || full.Contains("duplicate"))))
            return "Ese usuario o DNI ya está registrado.";

        if (full.Contains("documento") && (full.Contains("unique") || full.Contains("duplicate")))
            return "Ya hay una persona registrada con ese DNI.";

        if (full.Contains("23505") || full.Contains("duplicate key") || full.Contains("unique constraint"))
            return "Hay datos duplicados. Revisá DNI, email o usuario e intentá de nuevo.";

        if (full.Contains("foreign key") || full.Contains("23503"))
            return "Hay datos relacionados incompletos o inválidos. Revisá la información e intentá de nuevo.";

        // Genérico amigable — nunca el mensaje crudo de EF/Npgsql
        return "No se pudo completar la operación. Revisá los datos e intentá nuevamente.";
    }

    private static bool IsSafeBusinessMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return false;
        var m = message.ToLowerInvariant();
        if (m.Contains("exception") || m.Contains("stack") || m.Contains("npgsql")
            || m.Contains("entity framework") || m.Contains("see the inner")
            || m.Contains("dbupdate") || m.Contains("sqlstate") || m.Contains("23505"))
            return false;
        return message.Length <= 280;
    }
}
