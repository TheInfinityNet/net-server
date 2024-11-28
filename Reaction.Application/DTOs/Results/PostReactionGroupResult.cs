using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.Services.Reaction.Application.DTOs.Results
{
    public class PostReactionGroupResult
    {
        public Guid PostId { get; set; }
        public ReactionType Type { get; set; }
        public long Count { get; set; }
    }
}