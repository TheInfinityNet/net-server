using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IProfileRepository : ISqlRepository<Entities.Profile, Guid>
    {

        Task<List<Entities.Profile>> GetByType(ProfileType type);


    }
}
