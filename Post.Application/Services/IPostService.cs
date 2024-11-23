using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.Services
{
    public interface IPostService
    {
        public Task<Domain.Entities.Post> GetById(string id);

        public Task<Domain.Entities.Post> CreatePost(CreatePostBaseRequest request);

        public Task<Domain.Entities.Post> UpdatePost(UpdatePostRequest request);

        public Task<IEnumerable<Domain.Entities.Post>> GetAll();

        public Task<IList<Domain.Entities.Post>> GetAllByPresentationId(string presentationId);

        public Task<Domain.Entities.Post> DeletePost(string id);

        public Task<IList<Domain.Entities.Post>> GetByType(string type);

        public Task<IList<Domain.Entities.Post>> GetAllByOwnerId(string id);

        public Task<IList<string>> GetAllPresentationIds();

        public Task<IList<Domain.Entities.Post>> GetAllByParentId(string id);

        public Task<IList<Domain.Entities.Post>> GetAllByGroupId(string id);

        public Task<IList<string>> WhoCantSee(string id);

        public Task<CursorPagedResult<Domain.Entities.Post>> GetNewsFeed(string profileId, string cursor, int pageSize);

    }
}
