using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using InfinityNetServer.Services.Group.Domain.Repositories;

namespace InfinityNetServer.Services.Group.Application.GrpcServices
{
    public class GrpcGroupService : GroupService.GroupServiceBase
    {

        private readonly ILogger<GrpcGroupService> _logger;

        private readonly IGroupRepository _groupRepository;

        private readonly IGroupMemberRepository _groupMemberRepository;

        public GrpcGroupService(ILogger<GrpcGroupService> logger, IGroupRepository groupRepository, IGroupMemberRepository groupMemberRepository)
        {
            _logger = logger;
            _groupRepository = groupRepository;
            _groupMemberRepository = groupMemberRepository;
        }

        public override async Task<GetGroupIdsResponse> getGroupIds(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received getAccountIds request");
            var response = new GetGroupIdsResponse();
            var groups = await _groupRepository.GetAllAsync();
            response.Ids.AddRange(groups.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<GetGroupMembersWithGroupsResponse> getGroupMembersWithGroups(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received getGroupMembersWithGroups request");
            var response = new GetGroupMembersWithGroupsResponse();
            var groupMembers = await _groupMemberRepository.GetAllAsync();
            response.GroupMembersWithGroups.AddRange(groupMembers.Select(p => new GroupMemberWithGroup
            {
                UserProfileId = p.UserProfileId.ToString(),
                GroupId = p.GroupId.ToString()
            }));

            return await Task.FromResult(response);
        }

    }
}
