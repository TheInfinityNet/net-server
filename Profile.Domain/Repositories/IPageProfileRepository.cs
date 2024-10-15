using System;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IPageProfileRepository : ISqlRepository<PageProfile, Guid>
    {



    }
}
