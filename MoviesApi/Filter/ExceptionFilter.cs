using Microsoft.AspNetCore.Mvc.Filters;

namespace MoviesApi.Filter
{
    public class ExceptionFilter(ILogger<ExceptionFilter> logger) : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> logger = logger;

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, message: context.Exception.Message);
            base.OnException(context);
        }
    }
}
