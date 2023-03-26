using EventsITAcademy.Application.CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventsITAcademy.API.Infrastructure.Models
{
    public class ApiExceptionDetails : ProblemDetails
    {
        public const string UnhandledErrorCode = "UnhandledError";
        private HttpContext _context;
        private Exception _exception;

        public LogLevel LogLevel { get; set; }
        public string Code { get; set; }
        public string TraceId
        {
            get
            {
                if (Extensions.TryGetValue("TraceId", out var traceId))
                {
                    return (string)traceId;
                }
                return null;
            }
            set => Extensions["TraceId"] = value;
        }

        public ApiExceptionDetails(HttpContext context, Exception exception)
        {
            _context = context;
            _exception = exception;
            TraceId = context.TraceIdentifier;
            LogLevel = LogLevel.Error;
            Code = UnhandledErrorCode;
            Status = StatusCodes.Status500InternalServerError;
            Title = exception.Message;
            Instance = context.Request.Path;

            HandleException((dynamic)exception);
        }

        private void HandleException(ItemNotFoundException exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(InvalidLoginException exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.Unauthorized;
            Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(EventModificationException exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.Forbidden;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(ItemAlreadyExistsException exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.Conflict;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        //private void HandleException(UserHasNotOrderedPizzaException exception)
        //{
        //    Code = "UserHasNotOrderedPizza";
        //    Status = (int)HttpStatusCode.BadRequest;
        //    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.9";
        //    Title = exception.Message;
        //    LogLevel = LogLevel.Information;
        //}

        //private void HandleException(ConflictingUserAddressException exception)
        //{
        //    Code = "AddressDoesNotBelongToUser";
        //    Status = (int)HttpStatusCode.Conflict;
        //    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.9";
        //    Title = exception.Message;
        //    LogLevel = LogLevel.Information;
        //}
        private void HandleException(Exception exception)
        {

        }
    }
}
