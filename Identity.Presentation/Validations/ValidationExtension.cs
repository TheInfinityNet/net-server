using FluentValidation;
using InfinityNetServer.Services.Identity.Application.DTOs.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Identity.Presentation.Validations
{
    public static class ValidationExtension
    {

        public static void AddFluentValidation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IValidator<SignInRequest>, SignInRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<SignInRequestValidator>();
        }

    }
}
