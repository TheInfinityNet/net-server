using System;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IProfileRepository : ISqlRepository<Entities.Profile, Guid>
    {



    }
}
