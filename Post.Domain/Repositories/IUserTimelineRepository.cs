using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Domain.Repositories
{
    public interface IUserTimelineRepository : IMongoDbGenericRepository<UserTimeline, Guid>
    {

        public Task<CursorPagedResult<TimelinePost>> GetUserTimelineAsync(Guid profileId, string? cursor, int pageSize);

        public Task PushPostToTimelineAsync(Guid profileId, TimelinePost post);

        public Task<bool> IsExists(Guid profileId);

        public Task UpdateParentIdAsync(Guid profileId, Guid postId, Guid? parentId);

        public Task DeletePostFromTimelineAsync(Guid profileId, Guid postId);

    }
}
