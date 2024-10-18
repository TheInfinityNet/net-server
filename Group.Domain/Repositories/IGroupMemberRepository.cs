using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Group.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Group.Domain.Repositories
{
    public interface IGroupMemberRepository : ISqlRepository<GroupMember, Guid>
    {



    }
}
