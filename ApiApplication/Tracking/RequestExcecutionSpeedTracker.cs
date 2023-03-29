using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace ApiApplication.Tracking
{
    public class RequestExcecutionSpeedTracker : IActionFilter
    {
        private readonly ILogger<RequestExcecutionSpeedTracker> _logger;
        public RequestExcecutionSpeedTracker(ILogger<RequestExcecutionSpeedTracker> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Request to {context.HttpContext.Request.Path}. Start time: {DateTime.UtcNow}. Identifier: {context.HttpContext.TraceIdentifier}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Request to {context.HttpContext.Request.Path}. End time: {DateTime.UtcNow}. Identifier: {context.HttpContext.TraceIdentifier}");
        }
    }
}
