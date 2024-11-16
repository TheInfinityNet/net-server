using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Domain.Repositories
{
    public interface IUserTimelineRepository : IMongoDbGenericRepository<UserTimeline, Guid>
    {

        public Task<BCursorPagedResult<TimelinePost>> GetUserTimelineAsync(Guid profileId, string? cursor, int pageSize);

        public Task UpdateUserTimelineAsync(Guid profileId, TimelinePost post);

        public Task<bool> IsExists(Guid profileId);

    }
}
