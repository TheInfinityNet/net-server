using System;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;

namespace InfinityNetServer.Services.Profile.Infrastructure.Repositories
{
    public class ProfileRepository : SqlRepository<Domain.Entities.Profile, Guid>, IProfileRepository
    {

        public ProfileRepository(ProfileDbContext context) : base(context)
        { }

        

    }
}
