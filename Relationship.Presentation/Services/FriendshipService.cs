using System;
using InfinityNetServer.Services.Relationship.Application;
using System.Threading.Tasks;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using InfinityNetServer.Services.Relationship.Domain.Entities;

namespace InfinityNetServer.Services.Relationship.Presentation.Services
{
    public class FriendshipService(
        IFriendshipRepository friendshipRepository, 
        ILogger<FriendshipService> logger,
        IStringLocalizer<RelationshipSharedResource> localizer) : IFriendshipService
    {
        public async Task<int> CountFriendships(string profileId)
        {
            return await friendshipRepository.CountFriendshipsAsync(Guid.Parse(profileId));
        }

        public async Task<IList<string>> GetPreviewFriendIds(string profileId)
        {
            IList<Friendship> friendships = await friendshipRepository.GetFriendshipsWithLimitAsync(Guid.Parse(profileId), 10);

            return friendships.Select(f => 
                f.SenderId.ToString() == profileId ? f.ReceiverId.ToString() : f.SenderId.ToString()).ToList();
        }
    }
}
