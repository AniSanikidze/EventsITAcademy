using EventsITAcademy.API.Infrastructure.Middlewares;

namespace EventsITAcademy.API.Infrastructure.Extensions
{
    public static class ReqResLoggingExtensionMiddleware
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<RequestResponseLoggingMiddleware>();
            return applicationBuilder;
        }
    }
}
