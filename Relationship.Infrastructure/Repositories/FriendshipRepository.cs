using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Infrastructure.Data;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Repositories
{
    public class FriendshipRepository(RelationshipDbContext context) : SqlRepository<Friendship, Guid>(context) , IFriendshipRepository
    {

        public async Task<int> CountFriendshipsAsync(Guid profileId)
        {
            return await((RelationshipDbContext)_context).Friendships.CountAsync(f =>
                (f.SenderId == profileId || f.ReceiverId == profileId) &&
                f.Status == FriendshipStatus.Accepted);
        }

        public async Task<IList<Friendship>> GetFriendshipsWithLimitAsync(Guid profileId, int limit)
        {
            return await ((RelationshipDbContext)_context).Friendships
                .Where(f =>
                    (f.SenderId == profileId || f.ReceiverId == profileId) &&
                    f.Status == FriendshipStatus.Accepted).Take(limit).ToListAsync();
        }

        public async Task<bool> HasFriendship(Guid senderId, Guid receiverId)
        {
            return await ((RelationshipDbContext)_context).Friendships.AnyAsync(f =>
                (f.SenderId.Equals(senderId) && f.ReceiverId.Equals(receiverId)) ||
                (f.SenderId.Equals(receiverId) && f.ReceiverId.Equals(senderId)));
        }

    }
}
