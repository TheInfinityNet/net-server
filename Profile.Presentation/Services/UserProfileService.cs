using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.Interfaces;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Presentation.Services
{
    public class UserProfileService : IUserProfileService
    {

        private readonly IUserProfileRepository _userProfileRepository;

        private readonly ILogger<UserProfileService> _logger;

        private readonly IStringLocalizer<ProfileSharedResource> _localizer;

        public UserProfileService(IUserProfileRepository userProfileRepository, ILogger<UserProfileService> logger, IStringLocalizer<ProfileSharedResource> localizer)
        {
            _userProfileRepository = userProfileRepository;
            _logger = logger;
            _localizer = localizer;
        }

        public Task<UserProfile> GetUserProfileById(string id)
        {
            return _userProfileRepository.GetUserProfileByIdAsync(Guid.Parse(id));
        }

    }
}
