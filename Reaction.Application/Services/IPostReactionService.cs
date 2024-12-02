using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;
using InfinityNetServer.Services.Reaction.Application.DTOs.Results;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Application.Services
{
    public interface IPostReactionService
    {

        public Task<IList<(string postId, IDictionary<ReactionType, int> countDetails)>> CountByPostIdAsync(IList<string> postIds);

        public Task<IList<PostReaction>> GetAllByPostIdsAndProfileIds(IList<(string postId, string profileId)> postIdsAndProfileIds);

        public Task<CursorPagedResult<PostReaction>> GetByPostId(string postId, string cursor, int pageSize, ReactionType type);

        public Task<PostReaction> Create(AddPostReactionRequest request);

        public Task<List<PostReactionGroupResult>> GetPostReactions(string lstPostId);// Id cách nhau bởi ,

    }
}