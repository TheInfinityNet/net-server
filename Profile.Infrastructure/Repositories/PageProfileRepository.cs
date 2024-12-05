using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Infrastructure.Repositories
{
    public class PageProfileRepository(ProfileDbContext context) : SqlRepository<PageProfile, Guid>(context), IPageProfileRepository
    {

        public async Task<PageProfile> GetByAccountIdAsync(Guid accountId)
                    => await DbSet.FirstOrDefaultAsync(profile => profile.AccountId == accountId);

        public async Task<IList<PageProfile>> GetAllByIdsAsync(IEnumerable<Guid> ids)
            => await DbSet.Where(profile => ids.Contains(profile.Id)).ToListAsync();

    }
}
