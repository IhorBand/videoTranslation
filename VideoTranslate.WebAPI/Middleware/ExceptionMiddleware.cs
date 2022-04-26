using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VideoTranslate.Shared.Abstractions.Providers;
using VideoTranslate.WebApiClient.DTO;

namespace VideoTranslate.WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IResourceProvider resourceProvider;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, IResourceProvider resourceProvider, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.resourceProvider = resourceProvider;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (exception is ValidationException validationException)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        ErrorCode = "Validation_Error",
                        ErrorType = "Validation_Error",
                        ErrorMessage = validationException.Message,
                        Severity = "Warning"
                    }
                };

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(errors, Formatting.Indented)).ConfigureAwait(false);
            }
            else
            {
                var exceptionDetail = new ExceptionDetail
                {
                    ErrorMessage = this.resourceProvider.GetTextResourceById("UnhandledException"),
                    Message = exception.Message,
                    Source = exception.Source,
                    StackTrace = exception.StackTrace
                };

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(exceptionDetail, Formatting.Indented)).ConfigureAwait(false);
            }
        }
    }
}
