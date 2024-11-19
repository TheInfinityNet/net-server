using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IFriendshipService
    {

        public Task<bool> HasFriendship(string senderId, string receiverId, FriendshipStatus status);

        public Task<Friendship> GetByStatus(FriendshipStatus status, string senderId, string receiverId);

        public Task<IList<string>> GetFriendIds(string profile);

        public Task<int> CountFriendships(string profileId);

        public Task<IList<string>> GetPreviewFriendIds(string profileId);
        //public Task<IList<FriendSuggestionResponse>> GetFriendSuggestions(Guid currentUserId, Guid? nextCursor, int pageSize);
        public Task<PagedCursorResult<Guid>> GetPagedCommonFriendsAsync(Guid? currentUserId, Guid? cursor, int pageSize);
    }
}
