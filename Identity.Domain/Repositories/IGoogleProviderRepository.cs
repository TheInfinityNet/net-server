using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Domain.Repositories
{
    public interface IGoogleProviderRepository : ISqlRepository<GoogleProvider, Guid>
    {

        public Task<GoogleProvider> GetByAccountIdAsync(Guid accountId);

    }

}
