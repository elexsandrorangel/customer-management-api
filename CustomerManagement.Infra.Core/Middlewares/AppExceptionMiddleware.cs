using CustomerManagement.Infra.Core.Exceptions;
using System.Net;
using CustomerManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Infra.Core.Middlewares
{
    public class AppExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AppExceptionMiddleware> _logger;

        public AppExceptionMiddleware(RequestDelegate next, ILogger<AppExceptionMiddleware> logger)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            string errorMessage = exception.Message;
            ExceptionType exceptionType = ExceptionType.Error;
            HttpStatusCode statusCode;

            switch (exception)
            {
                case AppUnauthorizedException e1:
                    exceptionType = e1.ExceptionType;
                    statusCode = HttpStatusCode.Unauthorized;
                    break;

                case AppForbiddenException e2:
                    exceptionType = e2.ExceptionType;
                    statusCode = HttpStatusCode.Forbidden;
                    break;

                case AppNotFoundException e3:
                    exceptionType = e3.ExceptionType;
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case AppConflictException cex:
                    exceptionType = cex.ExceptionType;
                    statusCode = HttpStatusCode.Conflict;
                    break;

                case AppUnsupportedMediaTypeException e4:
                    exceptionType = e4.ExceptionType;
                    statusCode = HttpStatusCode.UnsupportedMediaType;
                    break;

                case AppTimeoutException e5:
                    exceptionType = e5.ExceptionType;
                    errorMessage = "Timeout";
                    statusCode = HttpStatusCode.GatewayTimeout;
                    break;

                case AppPreconditionFailedException e6:
                    exceptionType = e6.ExceptionType;
                    statusCode = HttpStatusCode.PreconditionFailed;
                    break;

                case AppUnprocessableEntityErrorException e7:
                    exceptionType = e7.ExceptionType;
                    statusCode = HttpStatusCode.UnprocessableEntity;
                    break;

                case AppException e:
                    exceptionType = e.ExceptionType;
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case NotImplementedException _:
                    errorMessage = "Resource not implemented";
                    statusCode = HttpStatusCode.NotImplemented;
                    break;

                default:
                    errorMessage = "Ops! Tivemos um problema, tente novamente mais tarde!";
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            try
            {
                switch (exceptionType)
                {
                    case ExceptionType.Info:
                        _logger.LogInformation(exception, "Request error - Info: {message}", errorMessage);
                        break;

                    case ExceptionType.Warning:
                        _logger.LogWarning(exception, "Request error - Warning: {message}", errorMessage);
                        break;

                    case ExceptionType.Error:
                        _logger.LogError(exception, "Request error - Error: {message}", errorMessage);
                        break;

                    case ExceptionType.Critical:
                        _logger.LogCritical(exception, "Request error - Critical: {message}", errorMessage);
                        break;
                }
            }
            catch (Exception)
            {
                _logger.LogError(exception, "Request error - Generic Error: {message}", errorMessage);
            }
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(new ResultViewModel
            {
                Success = false,
                Error = errorMessage,
                ExceptionType = exceptionType.ToString()
            }.ToString());
        }
    }
}
