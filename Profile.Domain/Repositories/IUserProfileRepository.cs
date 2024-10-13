using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IUserProfileRepository
    {

        Task CreateUserProfilesAsync(IEnumerable<UserProfile> userProfiles);

        Task<List<Guid>> GetAllUserProfileIdsAsync();

        Task<UserProfile> GetUserProfileByIdAsync(Guid id);

        Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile);

        Task UpdateUserProfileAsync(UserProfile userProfile);

    }
}
