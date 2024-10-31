using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class LocalProviderRepository(IdentityDbContext context) : SqlRepository<LocalProvider, Guid>(context), ILocalProviderRepository
    {

        public async Task<LocalProvider> GetByEmailAsync(string email) 
            => await context.LocalProviders.FirstOrDefaultAsync(x => x.Email == email);

    }

}
