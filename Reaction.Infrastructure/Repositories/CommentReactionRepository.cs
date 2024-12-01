using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Infrastructure.Repositories
{
    public class CommentReactionRepository(ReactionDbContext dbContext) : SqlRepository<CommentReaction, Guid>(dbContext), ICommentReactionRepository
    {
        public async Task<int> CountByCommentIdAndType(Guid commentId, ReactionType type)
            => await DbSet.CountAsync(x => x.CommentId == commentId && x.Type == type);

        public async Task<IList<CommentReaction>> GetAllByCommentIdsAndProfileIdsAsync(IList<(Guid commentId, Guid profileId)> commentIdsAndProfileIds)
        {
            var commentIds = commentIdsAndProfileIds.Select(x => x.commentId).ToList();
            var profileIds = commentIdsAndProfileIds.Select(x => x.profileId).ToList();

            return await DbSet
                .Where(reaction => !reaction.IsDeleted &&
                                   commentIds.Contains(reaction.CommentId) &&
                                   profileIds.Contains(reaction.ProfileId))
                .ToListAsync();
        }

    }
}
