using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Service.Interfaces;

namespace MarketPlace.Utilities.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //before
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potantialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>("ApiKey");
            if (!apiKey.Equals(potantialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
            //after
        }
    }

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //public class AtomicOperationPerUserAttribute : ActionFilterAttribute
    //{
    //    private readonly ILogger<AtomicOperationPerUserAttribute> _logger;
    //    private readonly IConcurrencyService _concurrencyService;

    //    public AtomicOperationPerUserAttribute(ILogger<AtomicOperationPerUserAttribute> logger, IConcurrencyService concurrencyService)
    //    {
    //        _logger = logger;
    //        _concurrencyService = concurrencyService;
    //    }

    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        var userId = Convert.ToInt32(context.HttpContext.User.Identity.GetUserId());

    //        var semaphore = _concurrencyService.SemaphorePerUser(userId);

    //        _logger.LogInformation($"User {userId} claims sempaphore with RequestId {context.HttpContext.TraceIdentifier}");

    //        semaphore.Wait();
    //    }

    //    public override void OnActionExecuted(ActionExecutedContext context)
    //    {
    //        var userId = Convert.ToInt32(context.HttpContext.User.Identity.GetUserId());

    //        var semaphore = _concurrencyService.SemaphorePerUser(userId);

    //        _logger.LogInformation($"User {userId} releases sempaphore with RequestId {context.HttpContext.TraceIdentifier}");

    //        semaphore.Release();
    //    }
    //}
}
