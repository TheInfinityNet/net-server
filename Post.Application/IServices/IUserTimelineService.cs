using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.IServices
{
    public interface IUserTimelineService
    {

        public Task<CursorPagedResult<TimelinePost>> GetUserTimeline(string profileId, string cursor, int pageSize);

        public Task UpdateUserTimeline(string profileId, TimelinePost post);

        public Task CreateIfNotExists(string profileId);

    }
}
