using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;
using InfinityNetServer.Services.Reaction.Application.DTOs.Results;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Application.Services
{
    public interface ICommentReactionService
    {

        public Task<IList<(string commentId, IDictionary<ReactionType, int> countDetails)>> CountByCommentIdAsync(IList<string> commentIds);

        public Task<IList<CommentReaction>> GetAllByCommentIdsAndProfileIds(IList<(string commentId, string profileId)> commentIdsAndProfileIds);

        public Task<CursorPagedResult<CommentReaction>> GetByCommentId(string commentId, string cursor, int pageSize, ReactionType type);

        public Task<CommentReaction> Create(AddCommentReactionRequest request);

        public Task<List<CommandReacionGroupResult>> GetCommandReaction(string lstCommandId);

    }
}