using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Infrastructure.Repositories
{
    public class PostPrivacyRepository(PostDbContext context)
        : SqlRepository<PostPrivacy, Guid>(context), IPostPrivacyRepository
    {

        public async Task<PostPrivacy> GetByPostIdAsync(Guid postId)
            => await DbSet.FirstOrDefaultAsync(p => p.PostId == postId);

    }
}
