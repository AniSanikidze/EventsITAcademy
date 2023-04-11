using EventsITAcademy.API.Infrastructure.Middlewares;

namespace EventsITAcademy.API.Infrastructure.Extensions
{
    public static class CultureMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<CultureMiddleware>();
            return applicationBuilder;
        }
    }
}
