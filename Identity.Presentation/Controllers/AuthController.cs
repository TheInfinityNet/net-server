using InfinityNetServer.BuildingBlocks.Application.Dtos;
using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.Services.Identity.Application.DTOs;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using InfinityNetServer.Services.Identity.Application.Interfaces;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Presentation.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IStringLocalizer<IdentitySharedResource> _localizer;

        private readonly ILogger<AuthenticationController> _logger;

        private readonly IConfiguration _configuration;

        private readonly IAuthService _authService;

        private readonly IPublishEndpoint _publishEndpoint;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IStringLocalizer<IdentitySharedResource> Localizer,
            IConfiguration configuration,
            IAuthService authService,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _localizer = Localizer;
            _configuration = configuration;
            _authService = authService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost("sign-in")]
        public IActionResult SignIn([FromBody] SignInRequest request)
        {
            if (request.Email == "admin@gmail.com" && request.Password == "admin123")
            {
                AccountProvider user = new AccountProvider
                {
                    Email = request.Email,
                    Account = new Account
                    {
                        Id = Guid.Parse("d55564f8-e09c-4d50-91c4-7d9d98b2f2d2")
                    }
                };

                var accessToken = _authService.GenerateToken(user.Account, false);
                var refreshToken = _authService.GenerateToken(user.Account, true);

                return Ok(new
                {
                    accessToken,
                    refreshToken
                });
            }
            throw new IdentityException(IdentityErrorCode.USER_NOT_FOUND, StatusCodes.Status400BadRequest);
        }

        [HttpPost("send-verification-mail")]
        public async Task<IActionResult> SendMail([FromBody] SendMailRequest request)
        {
            _logger.LogInformation(CultureInfo.CurrentCulture.ToString());
            await _publishEndpoint.Publish<IBaseContract<SendMailRequest>>(new
            {
                RoutingKey = "app.info",
                SendAt = DateTime.UtcNow,
                AcceptLanguage = CultureInfo.CurrentCulture.ToString(),
                Content = request
            });

            return Ok(new
            {
                message = _localizer["send_verification_email_success"].ToString()
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string bearerToken = Request.Headers["Authorization"].ToString()["Bearer ".Length..];
                var newAccessToken = await _authService.Refresh(request.RefreshToken!, bearerToken);

                return Ok(new
                {
                    message = _localizer["refresh_token_success"].ToString(),
                    accessToken = newAccessToken
                });
            }
            throw new IdentityException(IdentityErrorCode.TOKEN_MISSING, StatusCodes.Status422UnprocessableEntity);
        }

    }
}
