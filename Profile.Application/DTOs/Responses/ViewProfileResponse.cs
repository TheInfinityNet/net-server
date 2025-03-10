﻿using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Profile.Application.DTOs.Responses
{
    public class ViewProfileResponse<TProfileResponse> where TProfileResponse : BaseProfileResponse
    {

        public TProfileResponse Profile { get; set; }

        public IList<string> Actions { get; set; }

    }
}
