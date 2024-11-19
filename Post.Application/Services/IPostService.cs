using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.Services
{
    public interface IPostService
    {

        public Task<IList<string>> GetAllPresentationIds();

        public Task<Domain.Entities.Post> GetById(string id);

        public Task<IList<Domain.Entities.Post>> GetByType(string type);

        public Task<IList<string>> WhoCanSee(string id);

        public Task<IList<string>> WhoCantSee(string id);

        public Task<CursorPagedResult<Domain.Entities.Post>> GetNewsFeed(string profileId, string cursor, int pageSize);

    }
}
