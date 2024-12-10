using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.Services.Identity.Application.DTOs.Requests;
using InfinityNetServer.Services.Identity.Application.DTOs.Responses;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using InfinityNetServer.Services.Identity.Application.IServices;
using InfinityNetServer.Services.Identity.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
            CommonProfileClient profileClient,
            CommonFileClient fileClient,
            IMessageBus messageBus) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Sign up a new user")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            await authService.SignUp(request, false, messageBus);

            return Ok(new CommonMessageResponse
            (
                localizer["Message.SignUpSuccess"].ToString()
            ));
        }

        [EndpointDescription("Send verification email")]
        [ProducesResponseType(typeof(SendMailResponse), StatusCodes.Status200OK)]
        [HttpPost("send-mail")]
        public async Task<IActionResult> SendMail([FromBody] SendMailRequest request)
        {
            logger.LogInformation(CultureInfo.CurrentCulture.ToString());

            await authService.SendMailWithCode(request.Email, VerificationType.VerifyByCode, messageBus);

            return Ok(new SendMailResponse
            (
                localizer["Message.SendVerificationEmailSuccess"].ToString(),
                60
            ));
        }

        [EndpointDescription("Verify email by code")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyEmailByCodeRequest request)
        {

            await authService.VerifyEmailByCode(request.Email, request.Code, messageBus);

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
            var account = await authService.SignIn(request.Email, request.Password);
            var AccessToken = authService.GenerateToken(account, account.DefaultUserProfileId, false);
            var RefreshToken = authService.GenerateToken(account, account.DefaultUserProfileId, true);

            var userProfile = await profileClient.GetProfile(account.DefaultUserProfileId.ToString());
            userProfile.AccountId = account.Id;

            if (userProfile.Status == "Locked")
                throw new IdentityException(IdentityError.EMAIL_WAS_NOT_VERIFIED, StatusCodes.Status401Unauthorized);

            if (userProfile.Avatar != null)
                userProfile.Avatar = await fileClient.GetPhotoMetadata(userProfile.Avatar.Id.ToString());

            if (userProfile.Cover != null)
                userProfile.Cover = await fileClient.GetPhotoMetadata(userProfile.Cover.Id.ToString());

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

        [EndpointDescription("Sign in with social provider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("social")]
        public IActionResult SignInWithSocial([FromQuery] string provider = "Google")
        {
            if (!Enum.TryParse<ExternalProviderName>(provider, true, out var externalName))
            {
                logger.LogError("Invalid provider type");
                throw new IdentityException(IdentityError.INVALID_PROVIDER, StatusCodes.Status400BadRequest);
            }

            string url = authService.GenerateSocialAuthUrl(externalName);

            return Ok(new { Url = url });
        }

        [EndpointDescription("Social callback")]
        [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
        [HttpPost("social-callback")]
        public async Task<IActionResult> SocialCallback([FromBody] SocialCallbackRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.Provider))
            {
                logger.LogError("Invalid provider type");
                throw new IdentityException(IdentityError.INVALID_PROVIDER, StatusCodes.Status400BadRequest);
            }
            if (!Enum.TryParse<ExternalProviderName>(request.Provider.Trim(), true, out var externalName))
            {
                logger.LogError("Invalid provider type");
                throw new IdentityException(IdentityError.INVALID_PROVIDER, StatusCodes.Status400BadRequest);
            }

            var account = await authService.SocialCallback(request.Code, externalName, messageBus);
            var AccessToken = authService.GenerateToken(account, account.DefaultUserProfileId, false);
            var RefreshToken = authService.GenerateToken(account, account.DefaultUserProfileId, true);

            var userProfile = await profileClient.GetUserProfile(account.DefaultUserProfileId.ToString());
            userProfile.AccountId = account.Id;

            if (userProfile.Avatar != null)
                userProfile.Avatar = await fileClient.GetPhotoMetadata(userProfile.Avatar.Id.ToString());

            if (userProfile.Cover != null)
                userProfile.Cover = await fileClient.GetPhotoMetadata(userProfile.Cover.Id.ToString());

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
            var newAccessToken = await authService.Refresh(request.RefreshToken!);

            return Ok(new RefreshResponse
            (
                localizer["Message.RefreshTokenSuccess"].ToString(),
                newAccessToken
            ));
        }
    }

}
