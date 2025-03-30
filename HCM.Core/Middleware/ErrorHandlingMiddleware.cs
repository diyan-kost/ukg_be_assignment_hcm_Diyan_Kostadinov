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
                //httpContext.Items.Add("ErrorMessage", "An unexpected error occurred. Please try again later.");
                //httpContext.Response.HttpContext.Items.Add("ErrorMessage", "An unexpected error occurred. Please try again later.");
                httpContext.Session.SetString("ErrorMessage", ex.Message);
                httpContext.Response.Redirect("Index");
                //httpContext.Request.Path;
                //httpContext.Response.Redirect("/Home/Error");
            }
        }
    }
}
