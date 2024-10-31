using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IFriendshipService
    {

        Task<bool> HasFriendship(string senderId, string receiverId);

        Task<Friendship> GetByStatus(FriendshipStatus status, string senderId, string receiverId);

        Task<int> CountFriendships(string profileId);

        Task<IList<string>> GetPreviewFriendIds(string profileId);

    }
}
