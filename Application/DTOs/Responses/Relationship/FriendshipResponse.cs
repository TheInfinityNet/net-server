using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Relationship
{
    public class FriendshipResponse
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public PhotoMetadataResponse Avatar { get; set; }

        public int MutualFriendsCount { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
