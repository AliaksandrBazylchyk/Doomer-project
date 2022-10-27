using Application.Middlewares.ExceptionMiddlewareModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Application.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next, 
            ILogger<ExceptionMiddleware> logger
            )
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var result = new ErrorDetails
            {
                StatusCode = 500,
                Title = exception.Message
            };

            switch (exception)
            {
                case SessionException _:
                    result.StatusCode = 404;
                    break;
                case UserException _:
                    result.StatusCode = 400;
                    break;
            }
            _logger.LogError(JsonConvert.SerializeObject(result));
            context.Response.StatusCode = result.StatusCode;

            await context.Response.WriteAsync(result.ToString());
        }
    }
}
