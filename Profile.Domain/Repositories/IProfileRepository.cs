using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IProfileRepository : ISqlRepository<Entities.Profile, Guid>
    {

        public Task<IList<Entities.Profile>> GetAllByTypeAsync(ProfileType type);

        public Task<IList<Entities.Profile>> GetAllByIdsAsync(IEnumerable<Guid> ids);

        public Task<IList<Entities.Profile>> GetPotentialByLocationAsync(string location, int limit);

    }
}
