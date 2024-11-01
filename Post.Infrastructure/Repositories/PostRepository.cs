using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using System;

namespace InfinityNetServer.Services.Post.Infrastructure.Repositories
{
    public class PostRepository(PostDbContext context) 
        : SqlRepository<Domain.Entities.Post, Guid>(context), IPostRepository
    {



    }
}
