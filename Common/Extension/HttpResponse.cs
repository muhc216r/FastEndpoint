using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Common.Extension;
public static class HttpResponseExtension
{
    public static async Task SendErrors(this IPreProcessorContext context, string error, CancellationToken cancellation)
    {
        var content = new SendError(error, null);
        await context.HttpContext.Response.SendAsync(content, StatusCodes.Status400BadRequest, cancellation: cancellation);
    }

    public static async Task SendErrors(this IPreProcessorContext context, ValidationFailure[] errors, CancellationToken cancellation)
    {
        var content = new SendError(MessageResource.ValidationError,errors);
        await context.HttpContext.Response.SendAsync(content, StatusCodes.Status400BadRequest, cancellation: cancellation);
    }

    public static TService Resolve<TService>(this IPreProcessorContext context) where TService : class
    {
        return context.HttpContext.Resolve<TService>();
    }
}

public record SendError(string Message ,ValidationFailure[]? Errors);