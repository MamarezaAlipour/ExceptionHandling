using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExceptionHandling;

public class HandlerOptions
{
    public long LogMaxBodyLength { get; set; } = 4 * 1024;
    public string OverSizeBodyLengthMessage { get; set; } = "the request body is too large to record";
    public IResult OperationResult { get; set; } =
        new Result<object>(null, OException.DefaultMessage);
}

public static class ExceptionHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app) =>
        app.UseErrorHandler(new HandlerOptions());

    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app,
        HandlerOptions options)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>(options);
        return app;
    }

    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app,
        RequestDelegate exceptionHandler)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>(exceptionHandler);
        return app;
    }
}
