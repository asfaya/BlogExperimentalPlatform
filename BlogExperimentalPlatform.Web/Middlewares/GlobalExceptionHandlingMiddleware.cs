namespace BlogExperimentalPlatform.Web.Middlewares
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using BlogExperimentalPlatform.Utils;
    using Microsoft.AspNetCore.Http;

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalExceptionHandlingMiddleware
    {
        #region Members
        private readonly RequestDelegate _next;
        #endregion

        #region Constructor
        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        #region Methods
        public async Task Invoke(HttpContext httpContext)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = (int)HttpStatusCode.InternalServerError;
            if (exception is BlogSystemException)
                code = (int)HttpStatusCode.BadRequest;

            var result = exception.Message;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(result);
        }
        #endregion
    }
}
