using EventsITAcademy.API.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Diagnostics;

namespace EventsITAcademy.API.Infrastructure.Extensions
{
    public static class ExceptionHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExceptionHandlingMiddleware>();
            return applicationBuilder;
        }
    }
}
