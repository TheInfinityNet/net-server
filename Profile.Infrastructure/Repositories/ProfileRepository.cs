using System;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.Services.Profile.Infrastructure.Repositories
{
    public class ProfileRepository : SqlRepository<Domain.Entities.Profile, Guid>, IProfileRepository
    {

        public ProfileRepository(ProfileDbContext context) : base(context)
        { }

        public async Task<List<Domain.Entities.Profile>> GetByType(ProfileType type)
        {
            return await ((ProfileDbContext)_context).Profiles.Where(p => p.Type == type).ToListAsync();
        }

    }
}
