using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IFriendshipService
    {

        Task<bool> HasFriendship(string senderId, string receiverId, FriendshipStatus status);

        Task<Friendship> GetByStatus(FriendshipStatus status, string senderId, string receiverId);

        Task<int> CountFriendships(string profileId);

        Task<IList<string>> GetPreviewFriendIds(string profileId);
        //Task<IList<FriendSuggestionResponse>> GetFriendSuggestions(Guid currentUserId, Guid? nextCursor, int pageSize);
        Task<PagedCursorResult<Guid>> GetPagedCommonFriendsAsync(Guid? currentUserId, Guid? cursor, int pageSize);
    }
}
