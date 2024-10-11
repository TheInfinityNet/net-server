using InfinityNetServer.Services.Identity.Application;
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
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using InfinityNetServer.Services.Identity.Application.DTOs.Responses;
using InfinityNetServer.Services.Identity.Application.DTOs.Requests;

namespace InfinityNetServer.Services.Identity.Presentation.Controllers
{
    [Tags("Auth APIs")]
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

        [EndpointDescription("Sign up a new user")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpPost("sign-up")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            return Ok(new CommonMessageResponse
            (
                _localizer["sign_up_success"].ToString()
            ));
        }

        [EndpointDescription("Send verification email")]
        [ProducesResponseType(typeof(SendMailResponse), StatusCodes.Status200OK)]
        [HttpPost("send-mail")]
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

            return Ok(new SendMailResponse
            (
                _localizer["send_verification_email_success"].ToString(),
                60
            ));
        }

        [EndpointDescription("Verify email by code")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpPost("verify")]
        public IActionResult Verify([FromBody] VerifyEmailByCodeRequest request)
        {

            return Ok(new CommonMessageResponse
            (
                _localizer["verify_email_success"].ToString()
            ));
        }

        [EndpointDescription("Verify email by token")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpGet("verify")]
        public IActionResult Verify([FromQuery] string token)
        {

            return Ok(new CommonMessageResponse
            (
                _localizer["verify_email_success"].ToString()
            ));
        }

        [EndpointDescription("Reset password by code")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpPost("reset-by-code")]
        public IActionResult Reset([FromBody] ResetByCodeRequest request)
        {
            return Ok(new CommonMessageResponse
            (
                _localizer["reset_password_success"].ToString()
            ));
        }

        [EndpointDescription("Reset password by token")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpPost("reset-by-token")]
        public IActionResult Reset([FromQuery] string token, [FromBody] ResetByTokenRequest request)
        {
            return Ok(new CommonMessageResponse
            (
                _localizer["reset_password_success"].ToString()
            ));
        }

        [EndpointDescription("Sign in a user")]
        [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
        [HttpPost("sign-in")]
        public IActionResult SignIn([FromBody] SignInRequest request)
        {
            if (request.Email == "test@gmail.com" && request.Password == "test123")
            {
                AccountProvider user = new AccountProvider
                {
                    Email = request.Email,
                    Account = new Account
                    {
                        Id = Guid.Parse("d55564f8-e09c-4d50-91c4-7d9d98b2f2d2")
                    }
                };

                var AccessToken = _authService.GenerateToken(user.Account, false);
                var RefreshToken = _authService.GenerateToken(user.Account, true);

                return Ok(new SignInResponse
                (
                   new TokensResponse
                    (
                        AccessToken,
                        RefreshToken
                    ),
                   new UserProfileResponse
                    (
                        user.Account.Id,
                        user.Email,
                        "John",
                        "Doe",
                        "1234567890",
                        new DateTime(1990, 1, 1),
                        Gender.Male
                    )
                ));
            }
            throw new IdentityException(IdentityErrorCode.USER_NOT_FOUND, StatusCodes.Status400BadRequest);
        }

        [EndpointDescription("Refresh the access token")]
        [ProducesResponseType(typeof(RefreshResponse), StatusCodes.Status200OK)]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string bearerToken = Request.Headers["Authorization"].ToString()["Bearer ".Length..];
                var newAccessToken = await _authService.Refresh(request.RefreshToken!, bearerToken);

                return Ok(new RefreshResponse
                (
                    _localizer["refresh_token_success"].ToString(),
                    newAccessToken
                ));
            }
            throw new IdentityException(IdentityErrorCode.TOKEN_MISSING, StatusCodes.Status422UnprocessableEntity);
        }

    }
}
