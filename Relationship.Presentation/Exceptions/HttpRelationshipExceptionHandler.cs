﻿using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.Services.Relationship.Application;
using InfinityNetServer.Services.Relationship.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Presentation.Exceptions
{
    public class HttpRelationshipExceptionHandler(ILogger<HttpRelationshipExceptionHandler> logger, IStringLocalizer<RelationshipSharedResource> localizer) : IMiddleware
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
            string type = ErrorType.UnExpected.ToString();
            string message = localizer["UncategorizedError"].ToString();
            Dictionary<string, string> errors;

            //Đoạn này chia trương hợp định nghĩa error response tùy theo kiểu exception
            switch (exception)
            {
                case RelationshipException ex:
                    type = ex.Error.Type.ToString();
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

        private Dictionary<string, string> GetDetailedErrors(RelationshipError error)
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
