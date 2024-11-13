using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using System;

namespace InfinityNetServer.Services.Post.Infrastructure.Repositories
{
    public class PostRepository : SqlRepository<Domain.Entities.Post, Guid>, IPostRepository
    {
        public PostRepository(PostDbContext context) : base(context) { }
    }
}
