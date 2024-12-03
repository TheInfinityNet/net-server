using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Application.Services
{
    public interface ICommentReactionService
    {

        public Task<CommentReaction> GetByCommentIdAndProfileId(string commentId, string profileId);

        public Task<IList<(string commentId, IDictionary<ReactionType, int> countDetails)>> CountByCommentId(IList<string> commentIds);

        public Task<IList<CommentReaction>> GetAllByCommentIdsAndProfileIds(IList<(string commentId, string profileId)> commentIdsAndProfileIds);

        public Task<CursorPagedResult<CommentReaction>> GetByCommentId(string commentId, string cursor, int pageSize, ReactionType type);

        public Task<CommentReaction> Save(CommentReaction entity);

        public Task<CommentReaction> Delete(string postId, string profileId);

    }
}