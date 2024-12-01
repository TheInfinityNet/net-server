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
    public class PostReactionRepository(ReactionDbContext dbContext) : SqlRepository<PostReaction, Guid>(dbContext), IPostReactionRepository
    {

        public async Task<int> CountByPostIdAndType(Guid postId, ReactionType type)
            => await DbSet.CountAsync(x => x.PostId == postId && x.Type == type);

        public async Task<IList<PostReaction>> GetAllByPostIdsAndProfileIdsAsync(IList<(Guid postId, Guid profileId)> postIdsAndProfileIds)
        {
            var postIds = postIdsAndProfileIds.Select(x => x.postId).ToHashSet();
            var profileIds = postIdsAndProfileIds.Select(x => x.profileId).ToHashSet();

            return await DbSet
                .Where(reaction => !reaction.IsDeleted &&
                                   postIds.Contains(reaction.PostId) &&
                                   profileIds.Contains(reaction.ProfileId))
                .ToListAsync();
        }
    }
}
