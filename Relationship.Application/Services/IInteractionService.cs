using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IInteractionService
    {

        Task<bool> HasBlocked(string currentProfileId, string targetProfileId);

        Task<bool> HasFollowed(string currentProfileId, string targetProfileId);

        Task<bool> HasMuted(string currentProfileId, string targetProfileId);

        Task<bool> HasFriendRequest(string currentProfileId, string targetProfileId);

        Task<IList<Interaction>> GetByType(InteractionType type, string currentProfileId, string targetProfileId, int? limit);

    }
}
