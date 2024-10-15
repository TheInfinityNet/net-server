using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Identity.Domain.Repositories
{
    public interface IVerificationRepository : ISqlRepository<Verification, Guid>
    {



    }
}
