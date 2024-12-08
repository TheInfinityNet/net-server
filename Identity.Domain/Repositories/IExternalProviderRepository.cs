using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Domain.Repositories
{
    public interface IExternalProviderRepository : ISqlRepository<ExternalProvider, Guid>
    {

        public Task<ExternalProvider> GetByAccountIdAsync(Guid accountId);

        public Task<ExternalProvider> GetByAccountIdAndExternalNameAsync(Guid accountId, ExternalProviderName externalName);

    }

}
