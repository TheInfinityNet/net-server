using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Domain.Repositories
{
    public interface IVerificationRepository : ISqlRepository<Verification, Guid>
    {

        public Task<Verification> GetByCodeAndAccountIdAsync(string code, Guid accountId);

    }
}
