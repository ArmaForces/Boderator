using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ArmaForces.Boderator.BotService.Filters;

public class ExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        return Task.CompletedTask;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ArgumentNullException) HandleValidationError(context);
        if (context.Exception is NotImplementedException) HandleNotImplemented(context);
        if (context.ExceptionHandled is false) HandleOtherException(context);
    }

    private static void HandleNotImplemented(ExceptionContext context)
    {
        context.Result = new NotImplementedResult();
        context.ExceptionHandled = true;
    }

    private static void HandleValidationError(ExceptionContext context)
    {
        var error = new
        {
            Message = "Validation error",
            Details = context.Exception.Message
        };

        context.Result = new BadRequestObjectResult(error);
        context.ExceptionHandled = true;
    }

    private static void HandleOtherException(ExceptionContext context)
    {
        var error = new
        {
            Message = "Internal Server Error",
            Details = context.Exception.Message,
            Timestamp = DateTimeOffset.Now
        };

        context.Result = new ContentResult
        {
            Content = JsonConvert.SerializeObject(error),
            StatusCode = 500
        };
        context.ExceptionHandled = true;
    }
}
