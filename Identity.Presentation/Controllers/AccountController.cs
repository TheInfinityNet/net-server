using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.Services.Identity.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.Services.Identity.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.IServices;

namespace InfinityNetServer.Services.Identity.Presentation.Controllers
{
    [Tags("Account APIs")]
    [ApiController]
    [Route("accounts")]
    public class AccountController : BaseApiController
    {
        private readonly IStringLocalizer<IdentitySharedResource> _localizer;

        private readonly ILogger<AccountController> _logger;

        private readonly IConfiguration _configuration;

        private readonly IMessageBus _messageBus;

        public AccountController(
            IAuthenticatedUserService authenticatedUserService,
            ILogger<AccountController> logger,
            IStringLocalizer<IdentitySharedResource> Localizer,
            IConfiguration configuration) : base(authenticatedUserService)
        {
            _logger = logger;
            _localizer = Localizer;
            _configuration = configuration;
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

    }
}
