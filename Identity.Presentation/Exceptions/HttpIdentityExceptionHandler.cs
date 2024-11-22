using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            ErrorType type = ErrorType.UnExpected;
            string message = localizer["UncategorizedError"].ToString();
            Dictionary<string, string> errors;

            //Đoạn này chia trương hợp định nghĩa error response tùy theo kiểu exception
            switch (exception)
            {
                case IdentityException ex:
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

        private Dictionary<string, string> GetDetailedErrors(IdentityError errorCode)
        {
            return errorCode.ToString() switch
            {
                nameof(IdentityError.VALIDATION_ERROR) => new Dictionary<string, string>
                {
                    { "email", localizer[errorCode.Code].ToString() },
                    { "password", localizer[errorCode.Code].ToString() }
                },

                nameof(IdentityError.EXPIRED_PASSWORD) => new Dictionary<string, string>
                {
                    { "password", localizer[errorCode.Code].ToString() }
                },

                nameof(IdentityError.TOKEN_INVALID) => new Dictionary<string, string>
                {
                    { "token", localizer[errorCode.Code].ToString() }
                },

                nameof(IdentityError.WRONG_PASSWORD) => new Dictionary<string, string>
                {
                    { "password", localizer[errorCode.Code].ToString() }
                },

                nameof(IdentityError.PASSWORD_MISMATCH) => new Dictionary<string, string>
                {
                    { "password", localizer[errorCode.Code].ToString() }
                },
                nameof(IdentityError.EMAIL_ALREADY_IN_USE) => new Dictionary<string, string>
                {
                    { "email", localizer[errorCode.Code].ToString() }
                },

                nameof(IdentityError.WEAK_PASSWORD) => new Dictionary<string, string>
                {
                    { "password", localizer[errorCode.Code].ToString() }
                },

                nameof(IdentityError.INVALID_EMAIL) => new Dictionary<string, string>
                {
                    { "email", localizer[errorCode.Code].ToString() }
                },

                nameof(IdentityError.TERMS_NOT_ACCEPTED) => new Dictionary<string, string>
                {
                    { "termsAccepted", localizer[errorCode.Code].ToString() }
                },

                nameof(IdentityError.CODE_INVALID) => new Dictionary<string, string>
                {
                    { "code", localizer[errorCode.Code].ToString() }
                },
                _ => null
            };
        }


    }

}
