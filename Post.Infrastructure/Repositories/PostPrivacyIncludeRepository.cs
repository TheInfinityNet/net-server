using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using System;

namespace InfinityNetServer.Services.Post.Infrastructure.Repositories
{
    public class PostPrivacyIncludeRepository(PostDbContext context)
        : SqlRepository<PostPrivacyInclude, Guid>(context), IPostPrivacyIncludeRepository
    {



    }
}
