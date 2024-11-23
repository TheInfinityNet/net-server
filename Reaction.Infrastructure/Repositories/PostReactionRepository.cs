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
    public class PostReactionRepository(ReactionDbContext dbContext) : SqlRepository<PostReaction, Guid>(dbContext), IPostReactionRepository
    {

        public async Task<int> CountByPostIdAndType(Guid postId, ReactionType type)
            => await DbSet.CountAsync(x => x.PostId == postId && x.Type == type);

        public async Task<PostReaction> GetByPostIdAndProfileId(Guid postId, Guid profileId)
            => await DbSet.FirstOrDefaultAsync(x => x.PostId == postId && x.ProfileId == profileId);
    }
}
