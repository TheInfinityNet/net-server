using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.DTOs.Responses
{
    public class FriendSuggestionResponse { 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PhotoMetadataResponse Avatar { get; set; }
        public int MutualFriendsCount { get; set; }
        public FriendshipStatus Status { get; set; }
    }
}
