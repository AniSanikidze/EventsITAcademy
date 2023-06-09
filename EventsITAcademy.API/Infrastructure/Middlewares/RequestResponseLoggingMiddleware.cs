﻿using Serilog;
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
            await LogRequest(httpContext.Request).ConfigureAwait(false);

            var originalBody = httpContext.Response.Body;

            try
            {
                using (var memStream = new MemoryStream())
                {
                    httpContext.Response.Body = memStream;

                    await _next(httpContext).ConfigureAwait(false);

                    LogResponse(httpContext.Response, memStream);

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody).ConfigureAwait(false);
                }
            }
            finally
            {
                httpContext.Response.Body = originalBody;
            }
        }

        public async Task LogRequest(HttpRequest httpRequest)
        {
            var userIdClaim = "";
            var userRole = "";
            if (httpRequest.HttpContext.User.Claims.Any())
            {
                userIdClaim = httpRequest.HttpContext.User.FindFirst("UserId").Value;
                userRole = httpRequest.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            }
            var logInfo = $"*******Request Log*******{Environment.NewLine}" +
                $"IP = {httpRequest.HttpContext.Connection.RemoteIpAddress.ToString()}{Environment.NewLine}" +
                $"Scheme = {httpRequest.Scheme}{Environment.NewLine}" +
                $"Host = {httpRequest.Host.ToString()}{Environment.NewLine}" +
                $"IsSecured = {httpRequest.IsHttps}{Environment.NewLine}" +
                $"Method = {httpRequest.Method}{Environment.NewLine}" +
                $"Query String = {httpRequest.QueryString.ToString()}{Environment.NewLine}" +
                $"Path = {httpRequest.Path}{Environment.NewLine}" +
                $"Body = {await ReadRequestBody(httpRequest).ConfigureAwait(false)}{Environment.NewLine}" +
                $"Request Time = {DateTime.Now}{Environment.NewLine}" +
                $"UserId = {userIdClaim}{Environment.NewLine}" +
                $"User role = {userRole}";

            //var completePath = Directory.GetCurrentDirectory() + "\\Infrastructure\\Logging\\Logs.txt";
            //await File.AppendAllTextAsync(completePath, logInfo);
            Log.Information(logInfo);

        }

        private static async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[request.ContentLength ?? 0];
            await request.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;
            return bodyAsText;
        }
        public void LogResponse(HttpResponse httpResponse, MemoryStream memStream)
        {
            var userIdClaim = "";
            var userNameClaim = "";
            if (httpResponse.HttpContext.User.Claims.Any())
            {
                userIdClaim = httpResponse.HttpContext.User.FindFirst("UserId").Value;
                userNameClaim = httpResponse.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            }
            memStream.Position = 0;
            var responseBody = new StreamReader(memStream).ReadToEnd();
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
