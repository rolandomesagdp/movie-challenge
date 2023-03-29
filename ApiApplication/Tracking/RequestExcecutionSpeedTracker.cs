using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ApiApplication.Tracking
{
    public class RequestExcecutionSpeedTracker : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"Request to {context.HttpContext.Request.Path}. Start time: {DateTime.UtcNow}. Identifier: {context.HttpContext.TraceIdentifier}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"Request to {context.HttpContext.Request.Path}. End time: {DateTime.UtcNow}. Identifier: {context.HttpContext.TraceIdentifier}");
        }
    }
}
