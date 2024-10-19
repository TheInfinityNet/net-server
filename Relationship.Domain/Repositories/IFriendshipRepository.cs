using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Domain.Repositories
{
    public interface IFriendshipRepository : ISqlRepository<Friendship, Guid>
    {

        Task<bool> HasFriendship(Guid senderId, Guid receiverId);

    }
}
