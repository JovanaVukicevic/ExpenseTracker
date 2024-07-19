using ExpenseTracker.Service.CustomException;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

public class CustomExceptionFilter : IExceptionFilter
{
    private readonly ILogger<CustomExceptionFilter> _logger;

    public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError($"An unhandled exception occurred: {context.Exception}");
        if (context.Exception is NotFoundException)
        {
            context.Result = new NotFoundObjectResult(
                new { Message = context.Exception.Message }
            );
        }
        else if (context.Exception is BadHttpRequestException)
        {
            context.Result = new BadRequestObjectResult(
                new { Message = context.Exception.Message }
            );
        }
        else if (context.Exception is UnauthorizedAccessException)
        {
            context.Result = new UnauthorizedObjectResult(
                new { Message = context.Exception.Message }
            );
        }
        else
        {
            context.Result = new ObjectResult(
                new
                {
                    StatusCode = 500,
                    Message = context.Exception.Message
                }
            );
        }
        // var result = new ObjectResult(new
        // {
        //     StatusCode = 500,
        //     Message = "An error occurred while processing your request.",
        //     Detail = context.Exception.Message
        // });

        // result.StatusCode = 500;
        // context.Result = result;
    }
}