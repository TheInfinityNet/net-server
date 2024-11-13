using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using InfinityNetServer.Services.Identity.Application;

namespace InfinityNetServer.Services.Identity.Presentation.Exceptions
{
    public class HttpIdentityExceptionHandler(
        ILogger<HttpIdentityExceptionHandler> logger, 
        IStringLocalizer<IdentitySharedResource> localizer) : IMiddleware
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
            /*
            Đoạn này cấu hình trả về error response mặc định
                nếu như exception không phải kiểu được liệt kê trong đoạn switch case
            */
            context.Response.ContentType = "application/json";
            string type;
            string message = localizer["uncategorized"].ToString();
            Dictionary<string, string> errors;

            //Đoạn này chia trương hợp định nghĩa error response tùy theo kiểu exception
            switch (exception)
            {
                case IdentityException Ex:
                    type = Ex.ErrorCode.Code;
                    message = localizer[Ex.ErrorCode.Message].ToString();
                    errors = GetDetailedErrors(Ex.ErrorCode)!;

                    logger.LogError("App Exception: {Exception}", Ex);
                    context.Response.StatusCode = Ex.HttpStatus;
                    return context.Response.WriteAsJsonAsync(new
                    {
                        type,
                        message,
                        errors
                    });

                default:
                    logger.LogError("Unexpected Exception: {Exception}", exception);
                    break;
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsJsonAsync(new
            {
                message
            });
        }

        private Dictionary<string, string> GetDetailedErrors(IdentityErrorCode errorCode)
        {
            if (errorCode == IdentityErrorCode.VALIDATION_ERROR)
                return new Dictionary<string, string>
                {
                    { "email", localizer[errorCode.Message] },
                    { "password", localizer[errorCode.Message] }
                };

            else if (errorCode == IdentityErrorCode.EXPIRED_PASSWORD)
                return new Dictionary<string, string>
                {
                    { "password", localizer[errorCode.Message] }
                };

            else if (errorCode == IdentityErrorCode.TOKEN_INVALID)
                return new Dictionary<string, string>
                {
                    { "token", localizer[errorCode.Message] }
                };

            else if (errorCode == IdentityErrorCode.WRONG_PASSWORD)
                return new Dictionary<string, string>
                {
                    { "password", localizer[errorCode.Message] }
                };

            else if (errorCode == IdentityErrorCode.PASSWORD_MISMATCH)
                return new Dictionary<string, string>
                {
                    { "password", localizer[errorCode.Message] }
                };

            else if (errorCode == IdentityErrorCode.EMAIL_ALREADY_IN_USE)
                return new Dictionary<string, string>
                {
                    { "email", localizer[errorCode.Message] }
                };

            else if (errorCode == IdentityErrorCode.WEAK_PASSWORD)
                return new Dictionary<string, string>
                {
                    { "password", localizer[errorCode.Message] }
                };

            else if (errorCode == IdentityErrorCode.INVALID_EMAIL)
                return new Dictionary<string, string>
                {
                    { "email", localizer[errorCode.Message] }
                };

            else if (errorCode == IdentityErrorCode.TERMS_NOT_ACCEPTED)
                return new Dictionary<string, string>
                {
                    { "termsAccepted", localizer[errorCode.Message] }
                };

            else if (errorCode == IdentityErrorCode.CODE_INVALID)
                return new Dictionary<string, string>
                {
                    { "code", localizer[errorCode.Message] }
                };

            return null;
        }

    }

}
