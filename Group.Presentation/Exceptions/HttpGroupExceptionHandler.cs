using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using InfinityNetServer.Services.Group.Application;
using InfinityNetServer.Services.Group.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Group.Presentation.Exceptions
{
    public class HttpGroupExceptionHandler(ILogger<HttpGroupExceptionHandler> logger, IStringLocalizer<GroupSharedResource> localizer) : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        // Khi có lỗi xảy ra sẽ bay vô hàm này
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            ErrorType type = ErrorType.UnExpected;
            string message = localizer["UncategorizedError"].ToString();
            Dictionary<string, string> errors;

            //Đoạn này chia trương hợp định nghĩa error response tùy theo kiểu exception
            switch (exception)
            {
                case GroupException ex:
                    type = ex.Error.Type;
                    message = localizer[ex.Error.Code].ToString();
                    errors = GetDetailedErrors(ex.Error);

                    logger.LogError("App Exception: {Exception}", ex);
                    context.Response.StatusCode = ex.HttpStatus;
                    return context.Response.WriteAsJsonAsync(new
                    {
                        type,
                        message,
                        errors
                    });

                default:
                    logger.LogError("Unexpected Exception: {Exception}", exception);
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return context.Response.WriteAsJsonAsync(new
                    {
                        type,
                        message,
                        details = exception.Message
                    });
            }
        }

        private Dictionary<string, string> GetDetailedErrors(GroupError error)
        {
            return error.ToString() switch
            {
                //nameof(CommentError.INVALID_POST_TYPE) => new Dictionary<string, string>
                //{
                //    { "type", localizer[error.Code].ToString() }
                //},

                _ => null
            };
        }

    }

}
