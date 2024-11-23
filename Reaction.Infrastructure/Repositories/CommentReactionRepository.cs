using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Infrastructure.Repositories
{
    public class CommentReactionRepository(ReactionDbContext dbContext) : SqlRepository<CommentReaction, Guid>(dbContext), ICommentReactionRepository
    {
        public async Task<int> CountByCommentIdAndType(Guid commentId, ReactionType type)
            => await DbSet.CountAsync(x => x.CommentId == commentId && x.Type == type);

        public async Task<CommentReaction> GetByCommentIdAndProfileId(Guid commentId, Guid profileId)
            => await DbSet.FirstOrDefaultAsync(x => x.CommentId == commentId && x.ProfileId == profileId);
    }
}
