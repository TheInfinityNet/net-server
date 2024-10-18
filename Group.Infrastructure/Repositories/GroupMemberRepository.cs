using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using System;
using InfinityNetServer.Services.Group.Domain.Repositories;
using InfinityNetServer.Services.Group.Infrastructure.Data;
using InfinityNetServer.Services.Group.Domain.Entities;

namespace InfinityNetServer.Services.Group.Infrastructure.Repositories
{
    public class GroupMemberRepository : SqlRepository<GroupMember, Guid>, IGroupMemberRepository
    {
        public GroupMemberRepository(GroupDbContext context) : base(context) { }



    }
}
