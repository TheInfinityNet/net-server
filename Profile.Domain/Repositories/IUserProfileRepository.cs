using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IUserProfileRepository : ISqlRepository<UserProfile, Guid>
    {



    }
}
