using System;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Post.Domain.Entities;

namespace InfinityNetServer.Services.Post.Domain.Repositories
{
    public interface IPostPrivacyRepository : ISqlRepository<PostPrivacy, Guid>
    {

        

    }
}
