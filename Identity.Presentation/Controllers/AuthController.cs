using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.Services.Identity.Application.DTOs.Requests;
using InfinityNetServer.Services.Identity.Application.DTOs.Responses;
using InfinityNetServer.Services.Identity.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Presentation.Controllers
{
    [Tags("Auth APIs")]
    [ApiController]
    public class AuthController(
            IAuthenticatedUserService authenticatedUserService,
            ILogger<AuthController> logger,
            IStringLocalizer<IdentitySharedResource> localizer,
            IAuthService authService,
            IAccountService accountService,
            CommonProfileClient profileClient,
            IMessageBus messageBus) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Sign up a new user")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpPost("sign-up")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            logger.LogInformation(request.Gender.ToString());
            return Ok(new CommonMessageResponse
            (
                localizer["Message.SignUpSuccess"].ToString()
            ));
        }

        [EndpointDescription("Send verification email")]
        [ProducesResponseType(typeof(SendMailResponse), StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("send-mail")]
        public async Task<IActionResult> SendMail([FromBody] SendMailRequest request)
        {
            logger.LogInformation(CultureInfo.CurrentCulture.ToString());
            await messageBus.Publish(new DomainEvent.SendMailWithCodeEvent
            {
                Id = Guid.NewGuid(),
                ToMail = request.Email,
                Type = request.Type,
                AcceptLanguage = CultureInfo.CurrentCulture.ToString(),
                Code = "123456",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = GetCurrentProfileId()
            });

            return Ok(new SendMailResponse
            (
                localizer["Message.SendVerificationEmailSuccess"].ToString(),
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
                localizer["Message.VerifyEmailSuccess"].ToString()
            ));
        }

        [EndpointDescription("Verify email by token")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpGet("verify")]
        public IActionResult Verify([FromQuery] string token)
        {

            return Ok(new CommonMessageResponse
            (
                localizer["Message.VerifyEmailSuccess"].ToString()
            ));
        }

        [EndpointDescription("Sign in a user")]
        [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            var localProvider = await authService.SignIn(request.Email, request.Password);
            var account = await accountService.GetById(localProvider.AccountId.ToString());
            logger.LogInformation(localProvider.AccountId.ToString());
            var AccessToken = authService.GenerateToken(account, false);
            var RefreshToken = authService.GenerateToken(account, true);

            var userProfile = await profileClient.GetUserProfile(account.DefaultUserProfileId.ToString());
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
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                string bearerToken = value.ToString()["Bearer ".Length..];
                var newAccessToken = await authService.Refresh(request.RefreshToken!, bearerToken);

                return Ok(new RefreshResponse
                (
                    localizer["Message.RefreshTokenSuccess"].ToString(),
                    newAccessToken
                ));
            }
            throw new BaseException(BaseError.TOKEN_MISSING, StatusCodes.Status422UnprocessableEntity);
        }

    }
}
