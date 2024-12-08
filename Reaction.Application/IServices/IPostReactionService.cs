using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Application.IServices
{
    public interface IPostReactionService
    {

        public Task<PostReaction> GetById(string id);

        public Task<PostReaction> GetByPostIdAndProfileId(string postId, string profileId);

        public Task<IList<(string postId, IDictionary<ReactionType, int> countDetails)>> CountByPostIdAsync(IList<string> postIds);

        public Task<IList<PostReaction>> GetAllByPostIdsAndProfileIds(IList<(string postId, string profileId)> postIdsAndProfileIds);

        public Task<CursorPagedResult<PostReaction>> GetByPostId(string postId, string cursor, int pageSize, ReactionType type);

        public Task<PostReaction> Save(PostReaction entity, CommonPostClient postClient, IMessageBus messageBus);

        public Task<PostReaction> Delete(string postId, string profileId);

    }
}