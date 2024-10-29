using System.Collections.Generic;
using InfinityNetServer.Services.Profile.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.Services
{
    public interface IUserProfileService
    {

        Task<UserProfile> GetUserProfileById(string id);

        // Định nghĩa trong service interface
        Task<UserProfile> GetUserProfileByAccountId(string id);

        Task<IList<UserProfile>> GetUserProfilesByIds(IList<string> ids);

    }
}
