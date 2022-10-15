using Application.Middlewares;

namespace Application.Extensions
{
    public static class ExceptionExtension
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
