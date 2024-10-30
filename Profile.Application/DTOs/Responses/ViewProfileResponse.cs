using System.Collections.Generic;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Domain.Enums;

namespace InfinityNetServer.Services.Profile.Application.DTOs.Responses
{
    public class ViewProfileResponse<TProfileResponse> where TProfileResponse : BaseProfileResponse
    {

        public TProfileResponse Profile { get; set; }

        public IList<string> Actions { get; set; }

    }
}
