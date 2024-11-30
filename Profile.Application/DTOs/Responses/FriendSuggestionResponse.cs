using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using System;

namespace InfinityNetServer.Services.Profile.Application.DTOs.Responses
{
    public class FriendSuggestionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PhotoMetadataResponse Avatar { get; set; }
        public int MutualFriendsCount { get; set; }
        public string Status { get; set; }
    }
}
