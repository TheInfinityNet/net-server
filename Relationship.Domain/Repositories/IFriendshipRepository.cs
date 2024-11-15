﻿using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System.Linq;

namespace InfinityNetServer.Services.Relationship.Domain.Repositories
{
    public interface IFriendshipRepository : ISqlRepository<Friendship, Guid>
    {

        Task<bool> HasFriendship(Guid senderId, Guid receiverId, FriendshipStatus status);

        Task<Friendship> GetByStatus(FriendshipStatus status, Guid senderId, Guid receiverId);

        Task<int> CountFriendshipsAsync(Guid profileId);

        Task<IList<Friendship>> GetFriendshipsWithLimitAsync(Guid profileId, int? limit);
        Task<int> GetMutualFriendsCount(Guid profileId, Guid currentProfile);
        Task<IList<Guid>> GetFriendsOfCurrentUserAsync(Guid? currentUserId);
        Task<IList<Guid>> GetMutualFriendsAsync(Guid? currentUserId, IList<Guid> friendsOfCurrentUser);
        Task<IQueryable<Friendship>> GetMutualFriendsQueryAsync(Guid? currentUserId, IList<Guid> friendsOfCurrentUser, Guid? cursor);
        Task<IList<Friendship>> GetPagedResultsAsync(IQueryable<Friendship> query, int pageSize);
        (bool hasNext, bool hasPrevious, IList<Friendship> results) ProcessPagedResults(IList<Friendship> results, int pageSize, Guid? cursor);
        IList<Guid> GetCommonFriendsIds(IList<Friendship> results, Guid? currentUserId);
        
    }
}
