using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using InfinityNetServer.Services.Relationship.Application.Exceptions;
using InfinityNetServer.Services.Relationship.Application;

namespace InfinityNetServer.Services.Relationship.Presentation.Exceptions
{
    public class HttpRelationshipExceptionHandler : IMiddleware
    {

        private readonly ILogger<HttpRelationshipExceptionHandler> _logger;

        private readonly IStringLocalizer<RelationshipSharedResource> _localizer;

        public HttpRelationshipExceptionHandler(ILogger<HttpRelationshipExceptionHandler> logger, IStringLocalizer<RelationshipSharedResource> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

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
            string errorCode = "";
            string message = _localizer["uncategorized"].ToString();
            Dictionary<string, string> errors = [];

            //Đoạn này chia trương hợp định nghĩa error response tùy theo kiểu exception
            switch (exception)
            {
                case RelationshipException Ex:
                    errorCode = Ex.ErrorCode.Code;
                    message = _localizer[Ex.ErrorCode.Message].ToString();

                    _logger.LogError("App Exception: {Exception}", Ex);
                    context.Response.StatusCode = Ex.HttpStatus;
                    return context.Response.WriteAsJsonAsync(new
                    {
                        errorCode,
                        message
                    });

                default:
                    _logger.LogError("Unexpected Exception: {Exception}", exception);
                    break;
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsJsonAsync(new
            {
                message
            });
        }

    }

}
