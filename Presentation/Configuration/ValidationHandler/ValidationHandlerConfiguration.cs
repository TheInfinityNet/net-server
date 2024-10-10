using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.ValidationHandler;

public static class ValidationHandlerConfiguration
{

    public static void AddValidationHanlder(this IServiceCollection services, IConfiguration configuration, IStringLocalizer stringLocalizer)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                Dictionary<string, string> errors = context.ModelState
                        .Where(x => x.Value!.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(
                                e => stringLocalizer[e.ErrorMessage].ToString()).ToArray().First()
                        );
                BaseErrorCode commonErrorCode = BaseErrorCode.VALIDATION_ERROR;
                string errorCode = commonErrorCode.Code;
                string message = stringLocalizer[commonErrorCode.Message].ToString();

                return new BadRequestObjectResult(new
                {
                    errorCode,
                    message,
                    errors
                });

            };
        });
    }

}
