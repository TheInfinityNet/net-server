using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Domain.Repositories
{
    public interface ICommentReactionRepository : ISqlRepository<CommentReaction, Guid>
    {

        public Task<int> CountByCommentIdAndType(Guid commentId, ReactionType type);

        public Task<CommentReaction> GetByCommentIdAndProfileId(Guid commentId, Guid profileId);

    }
}
