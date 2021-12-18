using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace ExceptionHandling;

public class OExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = true;
        var exception = context.Exception;
        var logger = context.HttpContext.RequestServices.GetService<ILogger<OExceptionFilterAttribute>>();
        var operationResult = context.HttpContext.RequestServices.GetService<IResult>();

        string message;
        HttpStatusCode code;

        if (exception is OException)
        {
            message = exception.Message;
            code = HttpStatusCode.BadRequest;
            logger.LogWarning(exception, exception.Message);
        }
        else
        {
            message = operationResult.ErrorMessage;
            code = HttpStatusCode.InternalServerError;
            logger.LogError(exception, exception.Message);
        }

        operationResult.ErrorMessage = message;
        context.Result = new ObjectResult(JsonConvert.SerializeObject(operationResult, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })) { StatusCode = (int)code };
    }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        await Task.CompletedTask;
    }
}
