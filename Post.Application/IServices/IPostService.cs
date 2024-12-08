using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using System;
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

        public Task<Domain.Entities.Post> Create(Domain.Entities.Post entity, IMessageBus messageBus);

        public Task<Domain.Entities.Post> Update(Domain.Entities.Post entity, IMessageBus messageBus);

        public Task<Domain.Entities.Post> SoftDelete(string id);

        public Task<Domain.Entities.Post> Delete(string id);

        public Task<IEnumerable<Domain.Entities.Post>> GetAll();

        public Task<IList<Domain.Entities.Post>> GetAllByPresentationId(string presentationId);

        public Task<IList<Domain.Entities.Post>> GetAllByType(string type);

        public Task<IList<Domain.Entities.Post>> GetAllByOwnerId(string id);

        public Task<IList<string>> GetAllPresentationIds();

        public Task<IList<Domain.Entities.Post>> GetAllByParentId(string id);

        public Task<IList<Domain.Entities.Post>> GetAllByGroupId(string id);

        public Task<IList<string>> WhoCantSee(string id, string currentProfileId);

        public Task<CursorPagedResult<Domain.Entities.Post>> GetTimeline(string profileId, string cursor, int pageSize);

        public Task<CursorPagedResult<Domain.Entities.Post>> GetProfilePost(string currentProfileId, string profileId, string cursor, int pageSize);

        public Task<IList<BasePostResponse>> ToResponse(
            IList<Domain.Entities.Post> posts,
            Guid currentProfileId,
            CommonCommentClient commentClient,
            CommonReactionClient reactionClient,
            CommonFileClient fileClient, IMapper mapper);

        public Task<BasePostResponse> ToResponse(
            Domain.Entities.Post post,
            Guid currentProfileId,
            CommonCommentClient commentClient,
            CommonReactionClient reactionClient,
            CommonFileClient fileClient, IMapper mapper);

    }
}
