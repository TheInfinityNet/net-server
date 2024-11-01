using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using System;

namespace InfinityNetServer.Services.Post.Infrastructure.Repositories
{
    public class PostPrivacyRepository(PostDbContext context)
        : SqlRepository<PostPrivacy, Guid>(context), IPostPrivacyRepository
    {



    }
}
