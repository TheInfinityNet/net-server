using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.Services;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using MassTransit;
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

        // await async là kiến thức về bất đồng bộ có j ông search youtube xem thêm nha
        public async Task<UserProfile> GetUserProfileByAccountId(string id)
        {
            // chỗ này implement cái đã định nghĩa trong interface
            // truyền id vào là string nên phải parse ra Guid
            return await _userProfileRepository.GetUserProfileByAccountIdAsync(Guid.Parse(id));
        }

        public Task<UserProfile> GetUserProfileById(string id)
        {
            return _userProfileRepository.GetByIdAsync(Guid.Parse(id));
        }

    }
}
