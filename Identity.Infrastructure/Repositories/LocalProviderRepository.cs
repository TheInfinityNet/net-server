using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class LocalProviderRepository : SqlRepository<LocalProvider, Guid>, ILocalProviderRepository
    {

        public LocalProviderRepository(IdentityDbContext context) : base(context)
        { }

        public async Task<LocalProvider> GetByEmailAsync(string email)
        {
            return await ((IdentityDbContext)_context).LocalProviders.FirstOrDefaultAsync(x => x.Email == email);
        }

    }

}
