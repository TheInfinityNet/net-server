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
    public class ProfileRepository(ProfileDbContext context) : SqlRepository<Domain.Entities.Profile, Guid>(context), IProfileRepository
    {

        public async Task<List<Domain.Entities.Profile>> GetByType(ProfileType type) 
            => await ((ProfileDbContext)context).Profiles.Where(p => p.Type == type).ToListAsync();
        
    }
}
