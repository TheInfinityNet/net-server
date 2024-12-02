using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Application.Services
{
    public interface ICommentService
    {

        public Task<Domain.Entities.Comment> GetById(string id);

        public Task<int> CountByPostId(string postId);

        public Task<int> CountByParentId(string parentId);

        public Task<IList<int>> CountByParentIds(IList<string> parentIds);

        public Task<IList<Domain.Entities.Comment>> GetPopularComments(string postId);

        public Task<CursorPagedResult<Domain.Entities.Comment>> GetByPostId
            (string postId, string cursor, int pageSize, SortDirection sortDirection);

        public Task<CursorPagedResult<Domain.Entities.Comment>> GetReplies
            (string parentId, string cursor, int pageSize);

        public void ValidateType(Domain.Entities.Comment entity);

        public Task ConfirmSave(string id, string profileId, string fileMetadataId, IMessageBus messageBus);

        public Task<Domain.Entities.Comment> Create(Domain.Entities.Comment entity);

        public Task<Domain.Entities.Comment> Update(Domain.Entities.Comment entity);

        public Task<Domain.Entities.Comment> SoftDelete(string commentId);

    }
}
