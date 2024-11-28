using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.Services.Reaction.Application.DTOs.Results
{
    public class CommandReacionGroupResult
    {
        public Guid CommentId { get; set; }
        public ReactionType Type { get; set; }
        public long Count { get; set; }
    }
}