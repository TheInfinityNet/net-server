using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using System;
using InfinityNetServer.Services.Group.Domain.Repositories;
using InfinityNetServer.Services.Group.Infrastructure.Data;

namespace InfinityNetServer.Services.Group.Infrastructure.Repositories
{
    public class GroupRepository : SqlRepository<Domain.Entities.Group, Guid>, IGroupRepository
    {
        public GroupRepository(GroupDbContext context) : base(context) { }



    }
}
