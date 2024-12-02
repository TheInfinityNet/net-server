using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Domain.Repositories
{
    public interface ICommentReactionRepository : ISqlRepository<CommentReaction, Guid>
    {

        public Task<IList<(Guid commentId, IDictionary<ReactionType, int> countDetails)>> CountByCommentIdAsync(IList<Guid> commentIds);

        public Task<IList<CommentReaction>> GetAllByCommentIdsAndProfileIdsAsync(IList<(Guid commentId, Guid profileId)> commentIdsAndProfileIds);

    }
}
