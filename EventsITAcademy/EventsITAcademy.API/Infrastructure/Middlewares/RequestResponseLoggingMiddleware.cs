using Serilog;
using System.Security.Claims;
using System.Text;

namespace EventsITAcademy.API.Infrastructure.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await LogRequest(httpContext.Request);

            Stream originalBody = httpContext.Response.Body;

            try
            {
                using (var memStream = new MemoryStream())
                {
                    httpContext.Response.Body = memStream;

                    await _next(httpContext);

                    await LogResponse(httpContext.Response, memStream);

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }
            }
            finally
            {
                httpContext.Response.Body = originalBody;
            }
        }

        public async Task LogRequest(HttpRequest httpRequest)
        {
            string userIdClaim = "";
            string userNameClaim = "";
            if (httpRequest.HttpContext.User.Claims.Count() > 0)
            {
                userIdClaim = httpRequest.HttpContext.User.FindFirst("UserId").Value;
                userNameClaim = httpRequest.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            }
            var logInfo = $"*******Request Log*******{Environment.NewLine}" +
                $"IP = {httpRequest.HttpContext.Connection.RemoteIpAddress.ToString()}{Environment.NewLine}" +
                $"Scheme = {httpRequest.Scheme}{Environment.NewLine}" +
                $"Host = {httpRequest.Host.ToString()}{Environment.NewLine}" +
                $"IsSecured = {httpRequest.IsHttps}{Environment.NewLine}" +
                $"Method = {httpRequest.Method}{Environment.NewLine}" +
                $"Query String = {httpRequest.QueryString.ToString()}{Environment.NewLine}" +
                $"Path = {httpRequest.Path}{Environment.NewLine}" +
                $"Body = {await ReadRequestBody(httpRequest)}{Environment.NewLine}" +
                $"Request Time = {DateTime.Now}{Environment.NewLine}" +
                $"UserId = {userIdClaim}{Environment.NewLine}" +
                $"Username = {userNameClaim}";


            //var completePath = Directory.GetCurrentDirectory() + "\\Infrastructure\\Logging\\Logs.txt";
            //await File.AppendAllTextAsync(completePath, logInfo);
            Log.Information(logInfo);

        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[request.ContentLength ?? 0];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;
            return bodyAsText;
        }
        public async Task LogResponse(HttpResponse httpResponse, MemoryStream memStream)
        {
            string userIdClaim = "";
            string userNameClaim = "";
            if (httpResponse.HttpContext.User.Claims.Count() > 0)
            {
                userIdClaim = httpResponse.HttpContext.User.FindFirst("UserId").Value;
                userNameClaim = httpResponse.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            }
            memStream.Position = 0;
            string responseBody = new StreamReader(memStream).ReadToEnd();
            Console.WriteLine(responseBody);

            var logInfo = $"{Environment.NewLine}*******Response Log*******{Environment.NewLine}" +
                $"Content type = {httpResponse.ContentType}{Environment.NewLine}" +
                $"Status code = {httpResponse.StatusCode}{Environment.NewLine}" +
                $"Body = {responseBody}{Environment.NewLine}" +
                $"UserId = {userIdClaim}{Environment.NewLine}" +
                $"Username = {userNameClaim}{Environment.NewLine}" +
                $"Headers = ";

            httpResponse.Headers.ToList().ForEach(header => logInfo += $"{header.Key}:{header.Value}\n");
            logInfo += Environment.NewLine;
            //var completePath = Directory.GetCurrentDirectory() + "\\Infrastructure\\Logging\\Logs.txt";
            //await File.AppendAllTextAsync(completePath, logInfo);
            Log.Information(logInfo);
        }
    }
}
