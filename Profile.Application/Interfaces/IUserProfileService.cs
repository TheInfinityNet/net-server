using InfinityNetServer.Services.Profile.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.Interfaces
{
    public interface IUserProfileService
    {

        Task<UserProfile> GetUserProfileById(string id);

    }
}
