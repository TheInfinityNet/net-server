using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using InfinityNetServer.Services.Identity.Application.DTOs.Responses;
using InfinityNetServer.Services.Identity.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Identity.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.Services.Identity.Application.Services;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using Microsoft.AspNetCore.Authorization;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Identity.Presentation.Controllers
{
    [Tags("Auth APIs")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IStringLocalizer<IdentitySharedResource> _localizer;

        private readonly ILogger<AuthController> _logger;

        private readonly IConfiguration _configuration;

        private readonly IAuthService _authService;

        private readonly IAccountService _accountService;

        private readonly ProfileClient _profileClient;

        private readonly IMessageBus _messageBus;

        public AuthController(
            IAuthenticatedUserService authenticatedUserService,
            ILogger<AuthController> logger,
            IStringLocalizer<IdentitySharedResource> Localizer,
            IConfiguration configuration,
            IAuthService authService,
            IAccountService accountService,
            ProfileClient profileClient,
            IMessageBus messageBus) : base(authenticatedUserService)
        {
            _logger = logger;
            _localizer = Localizer;
            _configuration = configuration;
            _authService = authService;
            _accountService = accountService;
            _profileClient = profileClient;
            _messageBus = messageBus;
        }

        [EndpointDescription("Sign up a new user")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpPost("sign-up")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            _logger.LogInformation(request.Gender.ToString());
            return Ok(new CommonMessageResponse
            (
                _localizer["sign_up_success"].ToString()
            ));
        }

        [EndpointDescription("Send verification email")]
        [ProducesResponseType(typeof(SendMailResponse), StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("send-mail")]
        public async Task<IActionResult> SendMail([FromBody] SendMailRequest request)
        {
            _logger.LogInformation(CultureInfo.CurrentCulture.ToString());
            await _messageBus.Publish(new DomainEvent.SendMailWithCodeEvent
            {
                Id = Guid.NewGuid(),
                ToMail = request.Email,
                Type = request.Type,
                AcceptLanguage = CultureInfo.CurrentCulture.ToString(),
                Code = "123456",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = GetCurrentUserId()
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

        [EndpointDescription("Sign in a user")]
        [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            var localProvider = await _authService.SignIn(request.Email, request.Password);
            var account = await _accountService.GetById(localProvider.AccountId.ToString());
            _logger.LogInformation(localProvider.AccountId.ToString());
            var AccessToken = _authService.GenerateToken(account, false);
            var RefreshToken = _authService.GenerateToken(account, true);

            var userProfile = await _profileClient.GetUserProfile(account.DefaultUserProfileId.ToString());
            userProfile.AccountId = account.Id;

            return Ok(new SignInResponse
            (
                new TokensResponse
                (
                    AccessToken,
                    RefreshToken
                ),
                userProfile
            ));
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
            throw new CommonException(BaseErrorCode.TOKEN_MISSING, StatusCodes.Status422UnprocessableEntity);
        }

    }
}
