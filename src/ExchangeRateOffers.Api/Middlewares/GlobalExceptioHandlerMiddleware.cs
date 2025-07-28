using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ExchangeRateOffers.Api.Middlewares;

public class GlobalExceptioHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptioHandlerMiddleware> _logger;

    public GlobalExceptioHandlerMiddleware(ILogger<GlobalExceptioHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError
             (
                ex,
                "[{Time}] Ocurrió una excepción: {Reason}. Http Status Code: {StatusCode}",
                DateTime.UtcNow,
                ex.Message,
                HttpStatusCode.InternalServerError
            );

            ProblemDetails problemDetail = new()
            {
                Title = "An exception ocurred.",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                Status = StatusCodes.Status500InternalServerError,
                Instance = $"{context.Request.Method} {context.Request.Path}",
            };

            string problemDetailString = JsonSerializer.Serialize(problemDetail);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(problemDetailString);
        }
    }
}
