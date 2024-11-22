using System.Collections.Generic;
using InfinityNetServer.Services.Profile.Domain.Entities;
using System.Threading.Tasks;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;

namespace InfinityNetServer.Services.Profile.Application.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetUserProfileById(string id);

        Task<UserProfile> GetUserProfileByAccountId(string id);

        Task<IList<UserProfile>> GetUserProfilesByIds(IList<string> ids);

        Task<IList<UserProfile>> UpdateUserProfile(UpdateUserProfileRequest userProfile);
    }
}
