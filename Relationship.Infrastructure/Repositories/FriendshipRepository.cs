using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Infrastructure.Data;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Repositories
{
    public class FriendshipRepository : SqlRepository<Friendship, Guid>, IFriendshipRepository
    {
        public FriendshipRepository(RelationshipDbContext context) : base(context) { }


        public async Task<bool> HasFriendship(Guid senderId, Guid receiverId)
        {
            return await ((RelationshipDbContext)_context).Friendships.AnyAsync(f =>
                (f.SenderId.Equals(senderId) && f.ReceiverId.Equals(receiverId)) ||
                (f.SenderId.Equals(receiverId) && f.ReceiverId.Equals(senderId)));
        }

    }
}
