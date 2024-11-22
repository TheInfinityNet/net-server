using InfinityNetServer.Services.Reaction.Domain.Enums;
using System;

namespace InfinityNetServer.Application.DTOs.Results
{
    public class PostReactionGroupResult
    {
        public Guid PostId { get; set; }
        public ReactionType Type { get; set; }
        public long Count { get; set; }
    }
}