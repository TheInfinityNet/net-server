using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using System;

namespace InfinityNetServer.Services.Group.Domain.Repositories
{
    public interface IGroupRepository : ISqlRepository<Entities.Group, Guid>
    {



    }
}
