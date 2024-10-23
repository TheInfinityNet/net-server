using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text.Json;
using System.Text;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.Extensions.Localization;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;

namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.Jwt;

public static class JwtConfiguration
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration, IStringLocalizer stringLocalizer, bool isGateway = false)
    {
        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudiences = jwtOptions.Audiences,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.AccessKey)),
                };
                // Tắt ánh xạ các claim inbound
                options.MapInboundClaims = false;
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        string Token = context.Request.Headers["Authorization"].ToString()["Bearer ".Length..];
                        System.Console.WriteLine("Bearer token: " + Token);
                        if (isGateway)
                        {
                            string bearerToken = context.Request.Headers["Authorization"].ToString()["Bearer ".Length..];
                            var identityClient = context.HttpContext.RequestServices.GetRequiredService<CommonIdentityClient>();
                            System.Console.WriteLine("Bearer token: " + bearerToken);
                            if (!await identityClient.Introspect(bearerToken!))
                            {
                                BaseErrorCode errorCode = BaseErrorCode.TOKEN_INVALID;
                                context.Response.ContentType = "application/json";
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                var result = JsonSerializer.Serialize(
                                    new
                                    {
                                        errorCode = errorCode.Code,
                                        message = stringLocalizer[errorCode.Message].ToString()
                                    });
                                await context.Response.WriteAsync(result);
                            }

                        }
                    },
                    OnAuthenticationFailed = context =>
                    {
                        System.Console.WriteLine("OnAuthenticationFailed");
                        BaseErrorCode errorCode = BaseErrorCode.TOKEN_INVALID;
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        var result = JsonSerializer.Serialize(
                            new
                            {
                                errorCode = errorCode.Code,
                                message = stringLocalizer[errorCode.Message].ToString()
                            });
                        return context.Response.WriteAsync(result);
                    },
                    OnChallenge = context =>
                    {
                        System.Console.WriteLine("OnChallenge");
                        BaseErrorCode errorCode = BaseErrorCode.TOKEN_MISSING;
                        context.HandleResponse();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        var result = JsonSerializer.Serialize(
                            new
                            {
                                errorCode = errorCode.Code,
                                message = stringLocalizer[errorCode.Message].ToString()
                            });
                        return context.Response.WriteAsync(result);
                    }
                };
            });

        services.AddAuthorization();
    }
}
