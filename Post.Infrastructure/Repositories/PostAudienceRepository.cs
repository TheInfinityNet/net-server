using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Infrastructure.Repositories
{
    public class PostAudienceRepository(PostDbContext context)
        : SqlRepository<PostAudience, Guid>(context), IPostAudienceRepository
    {

        public async Task<PostAudience> GetByPostIdAsync(Guid postId)
            => await DbSet.FirstOrDefaultAsync(p => p.PostId == postId);

    }
}
