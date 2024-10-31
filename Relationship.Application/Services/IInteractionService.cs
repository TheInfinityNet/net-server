using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IInteractionService
    {

        Task<bool> HasBlocked(string currentProfileId, string targetProfileId);

        Task<bool> HasFollowed(string currentProfileId, string targetProfileId);

        Task<bool> HasMuted(string currentProfileId, string targetProfileId);

        Task<bool> HasFriendRequest(string currentProfileId, string targetProfileId);

    }
}
