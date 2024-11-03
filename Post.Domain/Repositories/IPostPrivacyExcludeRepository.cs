using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Post.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Post.Domain.Repositories
{
    public interface IPostPrivacyExcludeRepository : ISqlRepository<PostPrivacyExclude, Guid>
    {



    }
}
