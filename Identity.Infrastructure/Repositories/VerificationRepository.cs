using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class VerificationRepository(IdentityDbContext context) : SqlRepository<Verification, Guid>(context), IVerificationRepository
    {

        public async Task<Verification> GetByCodeAndAccountIdAsync(string code, Guid accountId)
            => await DbSet.FirstOrDefaultAsync(v => v.Code == code && v.AccountId == accountId);


    }
}
