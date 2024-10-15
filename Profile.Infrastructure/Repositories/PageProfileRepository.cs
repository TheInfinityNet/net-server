using System;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;

namespace InfinityNetServer.Services.Profile.Infrastructure.Repositories
{
    public class PageProfileRepository : SqlRepository<PageProfile, Guid>, IPageProfileRepository
    {

        public PageProfileRepository(ProfileDbContext context) : base(context)
        { }

        

    }
}
