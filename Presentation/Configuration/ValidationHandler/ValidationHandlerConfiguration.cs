using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.ValidationHandler;

public static class ValidationHandlerConfiguration
{

    public static void AddValidationHanlder(this IServiceCollection services, IConfiguration configuration, IStringLocalizer stringLocalizer)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                Dictionary<string, string[]> errors = context.ModelState
                        .Where(x => x.Value!.Errors.Count > 0)
                        .ToDictionary(
                            kvp => LowercaseFirstChar(kvp.Key),
                            kvp => kvp.Value!.Errors.Select(
                                e =>
                                {
                                    System.Console.WriteLine(e.GetType());
                                    return stringLocalizer[e.ErrorMessage].ToString();
                                }).ToArray()
                        );
                BaseErrorCode commonErrorCode = BaseErrorCode.VALIDATION_ERROR;
                string type = commonErrorCode.Code;
                string message = stringLocalizer[commonErrorCode.Message].ToString();

                return new BadRequestObjectResult(new
                {
                    type,
                    message,
                    errors
                });

            };
        });
    }

    /*public static void AddValidationHanlder(this IServiceCollection services, IConfiguration configuration, IStringLocalizer stringLocalizer)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => GetValidationErrorMessage(context, kvp.Key, kvp.Value!.Errors, stringLocalizer)
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
    }*/

    private static string LowercaseFirstChar(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
            return input;

        return char.ToLower(input[0]) + input.Substring(1);
    }

    private static IList<string> GetValidationErrorMessage(
        ActionContext context, string propertyName, ModelErrorCollection errors, IStringLocalizer stringLocalizer)
    {
        // Get the model being validated
        var modelType = context.ActionDescriptor.Parameters
            .FirstOrDefault(param => param.Name == propertyName)?
            .ParameterType;

        List<string> errorMessages = [];

        if (modelType != null)
        {
            var propertyInfo = modelType.GetProperty(propertyName);
            System.Console.WriteLine(modelType.Attributes);
            System.Console.WriteLine(modelType.GetProperty("BeforeFieldInit"));
            if (propertyInfo != null)
            {
                var attributes = propertyInfo.GetCustomAttributes(true);

                foreach (var error in errors)
                {
                    // Use switch statement to check for specific validation attributes
                    var attributeType = attributes.FirstOrDefault()?.GetType();
                    System.Console.WriteLine(attributeType);

                    if (attributes.OfType<MinLengthAttribute>().Any())
                    {
                        var minLengthAttribute = attributes.OfType<MinLengthAttribute>().FirstOrDefault();
                        System.Console.WriteLine(minLengthAttribute.ErrorMessage);
                        if (minLengthAttribute != null)
                        {
                            errorMessages.Add(stringLocalizer[minLengthAttribute.ErrorMessage!,
                                minLengthAttribute.Length].ToString()); // Use your existing message
                        }
                    }
                    else if (attributes.OfType<RangeAttribute>().Any())
                    {
                        var rangeAttribute = attributes.OfType<RangeAttribute>().FirstOrDefault();
                        System.Console.WriteLine(rangeAttribute.ErrorMessage);
                        if (rangeAttribute != null)
                        {
                            errorMessages.Add(stringLocalizer[rangeAttribute.ErrorMessage!,
                                    rangeAttribute.Minimum, rangeAttribute.Maximum]
                                .ToString()); // Use your existing message
                        }
                    }
                    else
                    {
                        System.Console.WriteLine(error.ErrorMessage);
                        errorMessages.Add(stringLocalizer[error.ErrorMessage].ToString()); // Fallback to default error message
                    }
                }
            }
        }

        return errorMessages; // Fallback if no specific error is found
    }

}
