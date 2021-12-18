using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace ExceptionHandling;

public class OExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
{
    private readonly ILogger logger;
    private readonly IResult operationResult;

    public OExceptionFilter(IResult operationResult, ILogger<OExceptionFilter> logger)
    {
        this.operationResult = operationResult;
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = true;
        var exception = context.Exception;

        string message;
        HttpStatusCode code;

        // expected exception
        if (exception is OException)
        {
            message = exception.Message;
            code = HttpStatusCode.BadRequest;
            logger.LogWarning(exception, exception.Message);
        }
        // unexpected exception
        else
        {
            message = operationResult.ErrorMessage;
            code = HttpStatusCode.InternalServerError;
            logger.LogError(exception, exception.Message);
        }

        operationResult.ErrorMessage = message;
        context.Result = new ObjectResult(JsonConvert.SerializeObject(operationResult, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })) { StatusCode = (int)code };
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        await Task.CompletedTask;
    }
}
