using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Infrastructure.Repositories
{
    public class PostRepository(PostDbContext context) : SqlRepository<Domain.Entities.Post, Guid>(context), IPostRepository
    {
        public async Task<IList<Guid>> GetAllPresentationIdsAsync()
            => await context.Posts.Where(p => p.PresentationId == null).Select(p => p.Id).ToListAsync();

        public async Task<IList<Domain.Entities.Post>> GetAllPresentationPostsAsync()
            => await context.Posts.Where(p => p.PresentationId == null && p.ParentId == null).ToListAsync();

        public async Task<IList<Domain.Entities.Post>> GetAllSharePostsAsync()
            => await context.Posts.Where(p => p.ParentId != null).ToListAsync();

        public async Task<IList<Domain.Entities.Post>> GetByTypeAsync(PostType type)
            => await context.Posts.Where(p => p.Type == type).ToListAsync();

    }
}
