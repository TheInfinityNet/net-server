using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.IServices
{
    public interface IPostService
    {

        public void ValidateMediaPostType(CreateMediaPostRequest dto);

        public void ValidateAudienceType(BasePostAudience dto);

        public Task ConfirmSave(string id, string profileId, string fileMetadataId, IMessageBus messageBus);

        public Task<Domain.Entities.Post> GetById(string id);

        public Task<Domain.Entities.Post> Create(Domain.Entities.Post entity);

        public Task<Domain.Entities.Post> UpdatePost(UpdatePostRequest request);

        public Task<IEnumerable<Domain.Entities.Post>> GetAll();

        public Task<IList<Domain.Entities.Post>> GetAllByPresentationId(string presentationId);

        public Task<Domain.Entities.Post> DeletePost(string id);

        public Task<IList<Domain.Entities.Post>> GetAllByType(string type);

        public Task<IList<Domain.Entities.Post>> GetAllByOwnerId(string id);

        public Task<IList<string>> GetAllPresentationIds();

        public Task<IList<Domain.Entities.Post>> GetAllByParentId(string id);

        public Task<IList<Domain.Entities.Post>> GetAllByGroupId(string id);

        public Task<IList<string>> WhoCantSee(string id);

        public Task<CursorPagedResult<Domain.Entities.Post>> GetNewsFeed(string profileId, string cursor, int pageSize);

    }
}
