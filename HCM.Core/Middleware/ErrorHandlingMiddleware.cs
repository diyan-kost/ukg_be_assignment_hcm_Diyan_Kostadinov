using FluentValidation;
using HCM.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HCM.Core.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An error occurred.");

                string errorMessage = string.Empty;

                if (ex is BaseCustomException)
                    errorMessage = ex.Message ?? "An error occurred! Please try again or contact us!";
                else if (ex is ValidationException validationException)
                    errorMessage = $"Incorrect input data! {validationException.Errors.FirstOrDefault()?.ErrorMessage}";
                else
                    errorMessage = "Oops, something went wrong! Please try again or contact us!";

                string redirectTo = ex is PermissionDeniedException ?
                    "/Index" :
                    httpContext.Request.Path.Value;

                httpContext.Session.SetString("ErrorMessage", errorMessage);
                httpContext.Response.Redirect(redirectTo);
            }
        }
    }
}
