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

                if (ex is EntityNotFoundException ||
                    ex is InvalidInputDataException)
                    errorMessage = ex.Message;
                else
                    errorMessage = "Oops, something went wrong! Please, try again or contact us!";


                httpContext.Session.SetString("ErrorMessage", ex.Message);
                httpContext.Response.Redirect("Index");
            }
        }
    }
}
