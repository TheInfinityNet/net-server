using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Domain.Repositories
{
    public interface IFacebookProviderRepository : ISqlRepository<FacebookProvider, Guid>
    {

        public Task<FacebookProvider> GetByAccountIdAsync(Guid accountId);

    }

}
