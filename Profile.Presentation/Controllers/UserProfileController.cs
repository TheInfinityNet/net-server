using InfinityNetServer.Services.Profile.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.BuildingBlocks.Application.Bus;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.Services.Profile.Application.Services;
using Microsoft.AspNetCore.Authorization;
using InfinityNetServer.Services.Profile.Domain.Entities;
using AutoMapper;

namespace InfinityNetServer.Services.Profile.Presentation.Controllers
{
    [Tags("User profile APIs")]
    [ApiController]
    [Route("users")]
    public class UserProfileController : BaseApiController
    {
        private readonly IStringLocalizer<ProfileSharedResource> _localizer;

        private readonly ILogger<UserProfileController> _logger;

        // khai báo thêm mapper
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        private readonly IUserProfileService _userProfileService;

        private readonly IMessageBus _messageBus;

        public UserProfileController(
            IAuthenticatedUserService authenticatedUserService,
            IStringLocalizer<ProfileSharedResource> localizer, 
            ILogger<UserProfileController> logger, 
            IMapper mapper,
            IConfiguration configuration, 
            IUserProfileService profileService, 
            IMessageBus messageBus) : base(authenticatedUserService)
        {
            _localizer = localizer;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _userProfileService = profileService;
            _messageBus = messageBus;
        }

        [EndpointDescription("Update user profile")]
        [HttpPut]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        public IActionResult UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            // TODO: Call the service to update the user profile
            // await _userProfileService.UpdateProfile(request);

            return Ok(new CommonMessageResponse
            (
                _localizer["profile_updated_success", request.Username].ToString()
            ));
        }

        [Authorize]
        [EndpointDescription("Retrieve user profile")]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> RetrieveProfile(string userId)
        {
            _logger.LogInformation("Retrieve user profile");

            UserProfile currentProfile = await _userProfileService.GetUserProfileById(userId);

            return Ok(_mapper.Map<UserProfileResponse>(currentProfile));
        }

    }
}
