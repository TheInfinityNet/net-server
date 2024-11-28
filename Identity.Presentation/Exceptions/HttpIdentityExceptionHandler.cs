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
            string type = ErrorType.UnExpected.ToString();
            string message = localizer["UncategorizedError"].ToString();
            Dictionary<string, string> errors;

            //Đoạn này chia trương hợp định nghĩa error response tùy theo kiểu exception
            switch (exception)
            {
                case IdentityException ex:
                    type = ex.Error.Type.ToString();
                    message = localizer[ex.Error.Code].ToString();
                    errors = GetDetailedErrors(ex.Error, nameof(ex.Error));

                    logger.LogError("App Exception: {Exception}", ex);
                    context.Response.StatusCode = ex.HttpStatus;
                    return context.Response.WriteAsJsonAsync(new
                    {
                        type,
                        message,
                        errors
                    });

                case BaseException ex:
                    type = ex.Error.Type.ToString();
                    message = localizer[ex.Error.Code].ToString();

                    logger.LogError("Base Exception: {Exception}", ex);
                    context.Response.StatusCode = ex.HttpStatus;
                    return context.Response.WriteAsJsonAsync(new
                    {
                        type,
                        message
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

        private Dictionary<string, string> GetDetailedErrors(IdentityError error, string errorName)
        {
            logger.LogError("Unexpected Exception: {Exception}", errorName);
            return error switch
            {
                IdentityError e when e == BaseError.VALIDATION_ERROR => new Dictionary<string, string>
                {
                    { "email", localizer[error.Code].ToString() },
                    { "password", localizer[error.Code].ToString() }
                },
                IdentityError e when e == IdentityError.EXPIRED_PASSWORD ||
                    e == IdentityError.WRONG_PASSWORD ||
                    e == IdentityError.PASSWORD_MISMATCH ||
                    e == IdentityError.WEAK_PASSWORD => new Dictionary<string, string>
                {
                    { "password", localizer[error.Code].ToString() }
                            },
                            IdentityError e when e == IdentityError.TOKEN_INVALID => new Dictionary<string, string>
                {
                    { "token", localizer[error.Code].ToString() }
                },
                            IdentityError e when e == IdentityError.EMAIL_ALREADY_IN_USE => new Dictionary<string, string>
                {
                    { "email", localizer[error.Code].ToString() }
                },
                            IdentityError e when e == IdentityError.INVALID_EMAIL => new Dictionary<string, string>
                {
                    { "email", localizer[error.Code].ToString() }
                },
                            IdentityError e when e == IdentityError.TERMS_NOT_ACCEPTED => new Dictionary<string, string>
                {
                    { "termsAccepted", localizer[error.Code].ToString() }
                },
                            IdentityError e when e == IdentityError.CODE_INVALID => new Dictionary<string, string>
                {
                    { "code", localizer[error.Code].ToString() }
                },
                _ => null
            };

        }


    }

}
