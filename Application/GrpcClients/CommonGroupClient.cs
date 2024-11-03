using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using static InfinityNetServer.BuildingBlocks.Application.Protos.GroupService;
using AutoMapper;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonGroupClient(GroupServiceClient client, ILogger<CommonGroupClient> logger, IMapper mapper)
    {

        public async Task<List<DTOs.Others.GroupMemberWithGroup>> GetGroupMemberWithGroup()
        {
            try
            {
                logger.LogInformation("Starting get group ids");
                var response = await client.getGroupMembersWithGroupsAsync(new Empty());
                // Call the gRPC server to introspect the token
                var result = response.GroupMembersWithGroups
                    .Select(_ => mapper.Map<DTOs.Others.GroupMemberWithGroup>(_)).ToList();

                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<List<string>> GetGroupIds()
        {
            try
            {
                logger.LogInformation("Starting get group ids");
                var response = await client.getGroupIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

    }
}
