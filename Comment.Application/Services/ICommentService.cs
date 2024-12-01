using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
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

        public Task<GetCommentsResponse> GetByPostId(GetCommentsRequest request);

        public void ValidateType(Domain.Entities.Comment entity);

        public Task ConfirmSave(string id, string profileId, string fileMetadataId, IMessageBus messageBus);

        public Task<Domain.Entities.Comment> Create(Domain.Entities.Comment entity);

        public Task<Domain.Entities.Comment> Update(Domain.Entities.Comment entity);

        public Task<Domain.Entities.Comment> SoftDelete(string commentId);

        //Task<List<ChildCommentResponse>> GetChildCommentsAsync(string parentCommentId);

    }
}
