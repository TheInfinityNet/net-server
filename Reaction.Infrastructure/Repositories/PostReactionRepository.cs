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

        public async Task<PostReaction> GetByPostIdAndProfileIdAsync(Guid postId, Guid profileId)
            => await DbSet.FirstOrDefaultAsync(reaction => !reaction.IsDeleted && reaction.PostId == postId && reaction.ProfileId == profileId);

        public async Task<IList<(Guid postId, IDictionary<ReactionType, int> countDetails)>> CountByPostIdAsync(IList<Guid> postIds)
        {
            var reactions = await DbSet
                .Where(reaction => !reaction.IsDeleted && postIds.Contains(reaction.PostId))
                .ToListAsync(); // Tải dữ liệu trước

                    return reactions
                        .GroupBy(r => r.PostId) // Nhóm theo PostId trên client-side
                        .Select(group => (
                            group.Key,
                            (IDictionary<ReactionType, int>)group
                                .GroupBy(r => r.Type) // Nhóm theo ReactionType
                                .ToDictionary(g => g.Key, g => g.Count())
                        ))
                        .ToList();
        }

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
