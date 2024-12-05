using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Profile.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IPageProfileRepository : ISqlRepository<PageProfile, Guid>
    {

        public Task<PageProfile> GetByAccountIdAsync(Guid accountId);

        public Task<IList<PageProfile>> GetAllByIdsAsync(IEnumerable<Guid> ids);

    }
}
