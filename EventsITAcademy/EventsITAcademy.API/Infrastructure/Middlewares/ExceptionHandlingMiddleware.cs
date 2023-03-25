using EventsITAcademy.API.Infrastructure.Models;
using Newtonsoft.Json;
using Serilog;

namespace EventsITAcademy.API.Infrastructure.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleException(exception, httpContext);
            }
        }

        public async Task HandleException(Exception exception, HttpContext httpContext)
        {
            var error = new ApiExceptionDetails(httpContext, exception);
            var result = JsonConvert.SerializeObject(error);
            var resultToLog = JsonConvert.SerializeObject(error);

            httpContext.Response.Clear();
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = error.Status.Value;

            await httpContext.Response.WriteAsync(result);
            //var completePath = Directory.GetCurrentDirectory() + "\\Infrastructure\\Logging\\ErrorLogs.txt";
            //await File.AppendAllTextAsync(completePath, $"{Environment.NewLine}*******Exception Log*******{Environment.NewLine}" + resultToLog);
            Log.Information(resultToLog);
        }
    }
}
