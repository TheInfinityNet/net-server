using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Profile.Domain.Enums;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IProfileRepository : ISqlRepository<Entities.Profile, Guid>
    {

        Task<List<Entities.Profile>> GetByType(ProfileType type);


    }
}
