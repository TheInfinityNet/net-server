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

        public async Task<CommentReaction> GetByCommentIdAndProfileIdAsync(Guid commentId, Guid profileId)
            => await DbSet.FirstOrDefaultAsync(reaction => !reaction.IsDeleted && reaction.CommentId == commentId && reaction.ProfileId == profileId);

        public async Task<IList<(Guid commentId, IDictionary<ReactionType, int> countDetails)>> CountByCommentIdAsync(IList<Guid> commentIds)
        {
            var reactions = await DbSet
                .Where(reaction => !reaction.IsDeleted && commentIds.Contains(reaction.CommentId))
                .ToListAsync(); // Tải dữ liệu trước

            return reactions
                .GroupBy(r => r.CommentId) // Nhóm theo PostId trên client-side
                .Select(group => (
                    group.Key,
                    (IDictionary<ReactionType, int>)group
                        .GroupBy(r => r.Type) // Nhóm theo ReactionType
                        .ToDictionary(g => g.Key, g => g.Count())
                ))
                .ToList();
        }

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
